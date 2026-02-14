import React, { useEffect, useState } from "react";
import { Button, Header, Image, Grid } from "semantic-ui-react";
import { Formik, Form } from "formik";
import * as yup from "yup";
import MyTextInput from "../../FormElements/MyTextInput";
import { useStore } from "../Store";
import { Loading } from "../Loader/Loader";
import ButtonNames from "../../Constants/button-names";
import {useNavigate} from 'react-router-dom';

export const Register = () => {
  const { userStore, Cofigurations } = useStore();
  const { loadConfigurations, configuration } = Cofigurations;
  const { register } = userStore;
  const navigate = useNavigate();

  useEffect(() => {
   loadConfigurations()
  }, []);

  const initialState = {
    email: "",
    password: "",
    confirmPassword: "",
    rememberMe: false,
  };

  const [registration, setRegistration] = useState(initialState);

  const handleFormsubmit = async (item) => {
    var url = await register(item);
    navigate(url);
  };
  const validationSchema = yup.object({});

  if (registration == null || configuration == null) return <Loading />;

  return (
    <Formik
      validationSchema={validationSchema}
      enableReinitialize
      initialValues={registration}
      onSubmit={(values) => handleFormsubmit(values)}
    >
      {({ handleSubmit, isSubmitting, dirty, isValid }) => (
        <Form className="ui form" onSubmit={handleSubmit}>
          <Grid
            textAlign="center"
            style={{ height: "80vh" }}
            verticalAlign="middle"
          >
            <Grid.Column style={{ maxWidth: 450 }}>
              <Header as="h2" color="teal" textAlign="center">
                {configuration != null && (
                  <Image
                    src={configuration.headerLogo}
                    style={{ height: "50px", width: "200px" }}
                  />
                )}
                Register
              </Header>
              <MyTextInput name="email" placeholder="E-mail address" />
              <MyTextInput
                name="password"
                placeholder="password"
                type="password"
              />

              <MyTextInput
                name="confirmPassword"
                placeholder=" Confirm Password"
                type="password"
              />

              <Button color="teal" type="submit" positive content={ButtonNames.Register} />
            </Grid.Column>
          </Grid>
        </Form>
      )}
    </Formik>
  );
};
