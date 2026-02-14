import React, { useEffect, useState } from "react";
import { Button, Label, Segment, Card, Image, Grid } from "semantic-ui-react";
import { Formik, Form } from "formik";
import * as yup from "yup";
import { useStore } from "../Store";
import "./Tags.css";
import MyTextInput from "../../FormElements/MyTextInput";
import MyTextArea from "../../FormElements/MyTextArea";
import MyCheckBox from "../../FormElements/MyCheckBox";
import MySelectInput from "../../FormElements/DropDown";
import MyDateInput from "../../FormElements/MyDatePicker";
import { useNavigate, useParams } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import { observer } from "mobx-react-lite";
import  ButtonKey  from "../../Constants/button-key";
import  LabelNames  from "../../Constants/label-names";
import  ButtonNames  from "../../Constants/button-names";
import RoutesConstants  from "../../Constants/routes";

export default observer(function NewsForm() {
  const { newsStore, categoryStore } = useStore();
  const { closeForm, createNews, updateNews, getNewsDetails } = newsStore;
  const { categories } = categoryStore;
  const navigate = useNavigate();
  const { id } = useParams();
  const [isDeleted, setIsDeleted] = useState(false);
  const [isFeatured, setIsFeatured] = useState(false);
  const [item, setItem] = useState({
    newsId: "",
    title: "",
    categoryId: 0,
    subTitle: "",
    content: "",
    image: "",
    video: "",
    expireDate: "",
    tags: "",
    isFeatured: false,
    isDeleted: true,
  });

  useEffect(() => {
    categoryStore.loadCategories();
    functionLoadData()
  }, []);
  const [tags, setTags] = useState([]);

  async function functionLoadData(){
    var data = await getNewsDetails(id);
      setItem(data);
      setIsDeleted(data?.isDeleted);
      setIsFeatured(data?.isFeatured);
  }

  const changefile = (event) => {
    let v = event.target.files;
    let reader = new FileReader();
    reader.readAsDataURL(v[0]);
    reader.onload = (e) => {
      setItem({ ...item, image: e.target?.result });
    };
  };

  function handleKeyDown(e) {
    if (e.key !== ButtonKey.Enter) return;
    const value = e.target.value;
    if (!value.trim()) return;
    setItem({ ...item, tags: item.tags + "," + value });
    e.target.value = "";
  }

  function removeTag(index) {
    setTags({ ...item, tags: item.tags.filter((el, i) => i !== index) });
  }

  const handleFormsubmit = (News) => {
    News.isDeleted = isDeleted;
    News.isFeatured = isFeatured;
    if (News.newsId == "") {
      createNews(News);
    } else {
      updateNews(News);
    }
    navigate(RoutesConstants.News);
  };
  function onKeyDown(keyEvent) {
    if ((keyEvent.charCode || keyEvent.keyCode) === ButtonKey.EnterNumber) {
      keyEvent.preventDefault();
    }
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
      key: categories[i].categoryId,
      value: categories[i].name,
    };
    categoptions.push(DropDownval);
  }

  if (item == null) return <Loading />;
  return (
    <Segment clearing>
      {item != null && (
        <Formik
          validationSchema={validationSchema}
          enableReinitialize
          initialValues={item}
          onSubmit={(values) => handleFormsubmit(values)}
        >
          {({ handleSubmit }) => (
            <Form
              className="ui form"
              onSubmit={handleSubmit}
              onKeyDown={onKeyDown}
            >
              <h4 class="ui dividing header">News Form</h4>
              <MyTextInput name="title" placeholder={"Name.."} />
              <MyTextInput name="subTitle" placeholder="SubTitle" />
              <MyTextArea name="content" placeholder="News Content" rows={5} />
              <MyTextInput name="video" placeholder={"Embeded Video.."} />

              <Grid>
                <Grid.Column width={3}>
                  <MyDateInput
                    name="expireDate"
                    placeholderText="expire date..."
                    maxDate={new Date("31/01/2022")}
                    isClearable
                  />
                </Grid.Column>
                <Grid.Column width={3}>
                  <MySelectInput
                    name="categoryId"
                    options={categoptions}
                    placeholder="Category..."
                  />
                </Grid.Column>

                <Grid.Column width={3}>
                  <MyCheckBox
                    name="isFeatured"
                    checked={isFeatured}
                    onClick={() => setIsFeatured(!isFeatured)}
                    label={LabelNames.IsFeatured}
                  />
                </Grid.Column>
                <Grid.Column width={3}>
                  <MyCheckBox
                    name="isDeleted"
                    checked={isDeleted}
                    onClick={() => setIsDeleted(!isDeleted)}
                    label={LabelNames.IsDeleted}
                  />
                </Grid.Column>
              </Grid>

              <Grid>
                <Grid.Row>
                  <div className="tags-input-container">
                    {item.tags != null &&
                      item.tags.split(",").map((tag, index) => (
                        <div className="tag-item" key={index}>
                          <span className="text">{tag}</span>
                          <span
                            className="close"
                            onClick={() => removeTag(index)}
                          >
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
                    <Image src={item.image} wrapped ui={false} />
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
                onClick={closeForm}
                floated="right"
                content={ButtonNames.Cancel}
              />
            </Form>
          )}
        </Formik>
      )}
    </Segment>
  );
});
