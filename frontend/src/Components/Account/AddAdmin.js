import { observer } from "mobx-react-lite";
import { useState } from "react";
import MySelectInput from "../../FormElements/DropDown";
import MyTextInput from "../../FormElements/MyTextInput";
import { useStore } from "../Store";
import { Button, Segment } from "semantic-ui-react";
import { Form, Formik } from "formik";
import * as yup from "yup";
import { useNavigate } from "react-router-dom";
import Roles from "../../Constants/roles";
import RoutesConstants from "../../Constants/routes";
import ValidationMessages from "../../Constants/validation-messages";
import ButtonNames from "../../Constants/button-names";

export default observer(function AdminForm() {
  const { userStore } = useStore();
  const { AddAdmin, addUser, selectedUser, EditUser } = userStore;
  const navigate = useNavigate();

  const initialState = selectedUser ?? {
    userId: "",
    userName: "",
    email: "",
    password: "",
    confirmPassword: "",
    role: "",
  };
  let roles = [];
  roles.push({
    text: Roles.Registered,
    key: Roles.Registered,
    value: Roles.Registered,
  });
  roles.push({ text: Roles.Admin, key: Roles.Admin, value: Roles.Admin });

  const [User, setUsers] = useState(initialState);

  const handleFormsubmit = async (Users) => {
    if (Users.userId == "") {
      if (Users.role == Roles.Admin) {
        await AddAdmin({
          id: User.userId,
          userName: Users.userName,
          email: Users.email,
          password: Users.password,
          confirmPassword: Users.confirmPassword,
        });
      } else {
        await addUser({
          id: User.userId,
          email: Users.email,
          password: Users.password,
          confirmPassword: Users.confirmPassword,
        });
      }
    } else {
      await EditUser({
        id: Users.userId,
        userName: Users.userName,
        userEmail: Users.email,
        password: Users.password,
        role: Users.role,
      });
    }
    navigate(RoutesConstants.Users);
  };
  const validationSchema = yup.object({
    userName: yup
      .string()
      .matches(/^[a-zA-Z0-9]{3,}$/, ValidationMessages.MoreThenThreeCharacters)
      .required(),
    email: yup.string().required(),
    role: yup.string().required(),
  });
  function sendTOAdmin() {
    navigate(RoutesConstants.Users);
  }
  return (
    <Segment clearing>
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={User}
        onSubmit={(values) => handleFormsubmit(values)}
      >
        {({ handleSubmit }) => (
          <Form className="ui form" onSubmit={handleSubmit}>
            <h4 class="ui dividing header">Add User</h4>
            <input type="hidden" name="userId" value={User.userId} />
            <MyTextInput name="userName" placeholder={"Name.."} />
            <MyTextInput name="email" type="email" placeholder="email" />
            <MyTextInput
              name="password"
              type="password"
              placeholder="password"
            />
            <MyTextInput
              name="confirmPassword"
              type="password"
              placeholder="confirm Password"
            />
            <MySelectInput
              name="role"
              options={roles}
              placholderText="select role ..."
            />
            <Button
              floated="right"
              positive
              type="submit"
              content={ButtonNames.Submit}
            />
            <Button
              floated="right"
              onClick={() => sendTOAdmin()}
              content={ButtonNames.Cancel}
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
