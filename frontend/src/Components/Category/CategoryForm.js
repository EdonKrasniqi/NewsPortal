import React, { useState } from "react";
import { Button, Segment } from "semantic-ui-react";
import { Formik, Form } from "formik";
import * as yup from "yup";
import { useStore } from "../Store";
import MyTextInput from "../../FormElements/MyTextInput";
import MyTextArea from "../../FormElements/MyTextArea";
import MyCheckBox from "../../FormElements/MyCheckBox";
import LabelNames from "../../Constants/label-names";
import ButtonNames from "../../Constants/button-names";

export const CategoryForm = () => {
  const { categoryStore } = useStore();
  const [online, setOnline] = useState(true);
  const { createCategory, updateCategory, selectedCategory, closeForm } =
    categoryStore;

  const initialState = selectedCategory ?? {
    categoryId: null,
    name : "",
    description: "",
    showOnline: true,
  };
  const [Category, setCategory] = useState(initialState);

  const handleFormsubmit = (Category) => {
    if (Category.categoryId == null) {
      createCategory(Category);
    } else {
      updateCategory(Category);
    }
  };

  const validationSchema = yup.object({
    name: yup.string().required(),
    description: yup.string().required(),
  });

  return (
    <Segment clearing>
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={Category}
        onSubmit={(values) => handleFormsubmit(values)}
      >
        {({ handleSubmit }) => (
          <Form className="ui form" onSubmit={handleSubmit}>
            <h4 class="ui dividing header">CategoryForm</h4>
            <MyTextInput name="name" placeholder={"Name.."} />
            <MyTextArea name="description" placeholder="description" rows={3} />
            <MyCheckBox
              checked={online}
              onClick={() => setOnline(!online)}
              name="showOnline"
              label={LabelNames.ShowOnline}
            />
            <Button floated="right" positive type="submit" content={ButtonNames.Submit} />
            <Button
              onClick={closeForm}
              floated="right"
              type="submit"
              content="cancel"
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
};
