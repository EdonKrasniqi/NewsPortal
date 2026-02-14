import React, { useState } from "react";
import { Button, Label, Segment, Card, Image, Grid } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { Formik, Form } from "formik";
import * as yup from "yup";
import { useStore } from "../Store";
import "./Tags.css";
import MyTextInput from "../../FormElements/MyTextInput";
import MyTextArea from "../../FormElements/MyTextArea";
import MyCheckBox from "../../FormElements/MyCheckBox";
import MySelectInput from "../../FormElements/DropDown";
import MyDateInput from "../../FormElements/MyDatePicker";
import { useNavigate } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import ButtonKey from "../../Constants/button-key";
import LabelNames from "../../Constants/label-names";
import ButtonNames from "../../Constants/button-names";
import RoutesConstants from "../../Constants/routes";

export default observer(function AddNews() {
  const { newsStore, categoryStore } = useStore();
  const { createNews, updateNews } = newsStore;
  const { categories } = categoryStore;
  const navigate = useNavigate();
  const [image, setimage] = useState();
  const [file, setFile] = useState();

  const model = {
    newsId: "",
    title: "",
    categoryId: "",
    subTitle: "",
    content: "",
    image: "",
    video: "",
    tags: "",
    isFeatured: false,
    isDeleted: false,
  };

  const [News, setNews] = useState(model);
  const [tags, setTags] = useState([]);

  const changefile = (event) => {
    let v = event.target.files;
    let reader = new FileReader();
    setFile(v[0]);
    reader.readAsDataURL(v[0]);
    reader.onload = (e) => {
      setimage(e.target?.result);
    };
  };
  function handleKeyDown(e) {
    if (e.key !== ButtonKey.Enter) return;
    const value = e.target.value;
    if (!value.trim()) return;
    setTags([...tags, value]);
    e.target.value = "";
  }

  function removeTag(index) {
    setTags({ ...tags.filter((el, i) => i !== index) });
  }

  const handleFormsubmit = (News) => {
    News.tags = tags.join(",");
    if (News.id == "") {
      createNews(News, file);
    } else {
      updateNews(News, file);
    }
    navigate(RoutesConstants.News);
  };
  function onKeyDown(keyEvent) {
    if ((keyEvent.charCode || keyEvent.keyCode) === 13) {
      keyEvent.preventDefault();
    }
  }

  function Index() {
    navigate(RoutesConstants.News);
  }
  const validationSchema = yup.object({
    title: yup.string().required(),
    subTitle: yup.string().required(),
    content: yup.string().required(),
    isFeatured: yup.string().required(),
    isDeleted: yup.string().required(),
  });
  let categoptions = [];

  for (var i = 0; i < categories.length; i++) {
    var DropDownval = {
      text: categories[i].name,
      key: categories[i].id,
      value: categories[i].id,
    };
    categoptions.push(DropDownval);
  }

  if (News == null) return <Loading />;
  return (
    <Segment clearing>
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={News}
        onSubmit={(values) => handleFormsubmit(values)}
      >
        {({ handleSubmit }) => (
          <Form
            className="ui form"
            onSubmit={handleSubmit}
            onKeyDown={onKeyDown}
          >
            <h4 class="ui dividing header">{LabelNames.NewsForm}</h4>

            <MyTextInput name="title" placeholder={"Name.."} />
            <MyTextInput name="subTitle" placeholder="SubTitle" />
            <MyTextArea name="content" placeholder="News Content" rows={5} />
            <MyTextInput name="video" placeholder={"Embeded Video.."} />
            <Grid>
              <Grid.Column width={3}>
                <MySelectInput
                  name="categoryId"
                  options={categoptions}
                  placeholder="Category..."
                />
              </Grid.Column>
              <Grid.Column width={3}>
                <MyCheckBox name="isFeatured" label={LabelNames.IsFeatured} />
              </Grid.Column>
              <Grid.Column width={3}>
                <MyCheckBox name="isDeleted" label={LabelNames.IsDeleted} />
              </Grid.Column>
            </Grid>
            <Grid>
              <Grid.Row>
                <div className="tags-input-container">
                  {tags.map((tag, index) => (
                    <div className="tag-item" key={index}>
                      <span className="text">{tag}</span>
                      <span className="close" onClick={() => removeTag(index)}>
                        &times;
                      </span>
                    </div>
                  ))}
                  <input
                    onKeyDown={handleKeyDown}
                    type="text"
                    className="tags-input"
                    placeholder="Type somthing"
                  />
                </div>
              </Grid.Row>

              <Grid.Row centered>
                <Card>
                  <Image src={image ?? News.image} wrapped ui={false} />
                  <Card.Content></Card.Content>

                  <Card.Content extra>
                    <input
                      type="file"
                      hidden
                      style={{ marginTop: "200px" }}
                      id="file"
                      onChange={changefile}
                    />
                    <label
                      for="file"
                      className="btn"
                      style={{ marginTop: "10px" }}
                    >
                      <Label content={LabelNames.UploadImage} color="blue" />
                    </label>
                  </Card.Content>
                </Card>
              </Grid.Row>
            </Grid>
            <Button
              floated="right"
              positive
              type="subimit"
              content={ButtonNames.Submit}
            />
            <Button
              onClick={() => Index()}
              floated="right"
              content={ButtonNames.Cancel}
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
