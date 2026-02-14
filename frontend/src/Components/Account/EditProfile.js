import React from "react";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useState } from "react";
import { Button, Label, Segment, Card, Image } from "semantic-ui-react";
import MyTextInput from "../../FormElements/MyTextInput";
import { Form, Formik } from "formik";
import * as yup from "yup";
import { useStore } from "../Store";
import { useNavigate } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import ValidationMessages from "../../Constants/validation-messages";
import RoutesConstants from "../../Constants/routes";
import LabelNames from "../../Constants/label-names";

export default observer(function EditProfile() {
  const { userStore } = useStore();
  const { EditUser,currentUserId,usersById } = userStore;
  const [user, setUser] = useState(null);
  const navigate = useNavigate();
  useEffect(() => {
    getData();
  }, []);

  const [image, setimage] = useState();
  const [file, setFile] = useState();

  async function getData() {
    await this.loadUser()
    setUser(usersById(currentUserId));
  }
  const validationSchema = yup.object({
    userName: yup
      .string()
      .matches(/^[a-zA-Z0-9]{3,}$/, ValidationMessages.MoreThenThreeCharacters)
      .required(),
    email: yup.string().required(),
    role: yup.string().required(),
  });
  function sendTOAdmin() {
    navigate(RoutesConstants.HomePageRoute);
  }

  const changefile = (event) => {
    let v = event.target.files;
    setFile(v[0]);
    let reader = new FileReader();
    reader.readAsDataURL(v[0]);
    reader.onload = (e) => {
      setimage(e.target?.result);
    };
  };

  const handleFormsubmit = async (user) => {
    EditUser(user, file);
  };

  if (user == null) return <Loading />;
  return (
    <React.Fragment>
      <Segment clearing>
        <Formik
          validationSchema={validationSchema}
          enableReinitialize
          initialValues={user}
          onSubmit={(values) => handleFormsubmit(values)}
        >
          {({ handleSubmit, isSubmitting, dirty, isValid }) => (
            <Form className="ui form" onSubmit={handleSubmit}>
              <h4 class="ui dividing header">Edit Profile</h4>
              <input type="hidden" name="Id" value={user.userId} />
              <MyTextInput name="userName" placeholder={"Name.."} />
              <MyTextInput name="email" type="email" placeholder="email" />
              <MyTextInput name="phoneNumber" placeholder={"phone Number.."} />

              <Card>
                <Image src={image ?? user.avatar} wrapped ui={false} />
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

              <Button floated="right" positive type="submit" content="submit" />
              <Button
                floated="right"
                onClick={() => sendTOAdmin()}
                content="cancel"
              />
            </Form>
          )}
        </Formik>
      </Segment>
    </React.Fragment>
  );
});
