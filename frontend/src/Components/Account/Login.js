import React, { useEffect, useState } from "react";
import {
  Button,
  Header,
  Segment,
  Image,
  Grid,
  Message,
} from "semantic-ui-react";
import { Formik, Form } from "formik";
import * as yup from "yup";
import MyTextInput from "../../FormElements/MyTextInput";
import { useStore } from "../Store";
import { Loading } from "../Loader/Loader";
import { useNavigate } from "react-router-dom";
import { observer } from "mobx-react-lite";
import  ButtonNames  from "../../Constants/button-names";
import  Colors  from "../../Constants/color";

export default observer(function Login() {
  const { userStore, Cofigurations } = useStore();
  const { login } = userStore;
  const { loadConfigurations, configuration } = Cofigurations;
  const navigate = useNavigate();
  const [configurations, setConfigurations] = React.useState(null);

  useEffect(() => {
    loadConfigurations();
    setConfigurations(configuration[0]);
  }, []);

  const initialState = {
    email: "",
    password: "",
  };

  const [registration, setRegistration] = useState(initialState);
  const handleFormsubmit = async (x) => {
    x.rememberMe = false;
    await login(x);
    navigate("/");
  };
  const validationSchema = yup.object({
    email: yup.string().required(),
    password: yup.string().required(),
  });

  if (registration == null || configurations == null) return <Loading />;
  return (
    <Formik
      validationSchema={validationSchema}
      enableReinitialize
      initialValues={registration}
      onSubmit={(values) => handleFormsubmit(values)}
    >
      {({ handleSubmit }) => (
        <Form
          className="ui form"
          style={{ backgroundColor: Colors.ShadeGray }}
          onSubmit={handleSubmit}
        >
          <Grid
            textAlign="center"
            style={{ height: "80vh" }}
            verticalAlign="middle"
          >
            <Grid.Column style={{ maxWidth: 450 }}>
              <Header as="h2" color={Colors.teal} textAlign="center">
                {configurations && (
                  <Image
                    src={""}
                    style={{ height: "50px", width: "200px" }}
                  />
                )}{" "}
                {ButtonNames.Login}
              </Header>

              <Segment stacked>
                <MyTextInput
                  fluid
                  icon="user"
                  iconPosition="left"
                  name="email"
                  placeholder="E-mail address"
                />
                <MyTextInput
                  fluid
                  icon="lock"
                  iconPosition="left"
                  placeholder="Password"
                  name="password"
                  type="password"
                />

                <Button color="teal" type="submit" fluid size="large">
                  {ButtonNames.Login}
                </Button>
              </Segment>

              <Message>
                <a href="/forgotpassword">{ButtonNames.ForgotPassword}</a>
              </Message>
            </Grid.Column>
          </Grid>
        </Form>
      )}
    </Formik>
  );
});
