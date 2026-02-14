import React, { useState } from "react";
import { Button, Label, Grid, Table } from "semantic-ui-react";
import { Formik, Form } from "formik";
import * as yup from "yup";
import MyDateInput from "../../FormElements/MyDatePicker";
import MySelectInput from "../../FormElements/DropDown";
import LabelNames from "../../Constants/label-names";
import ButtonNames from "../../Constants/button-names";
import { useStore } from "../Store";
import Colors from "../../Constants/color";

export default function FirstRaport() {
  const { Report } = useStore();
  const { getReportModel } = Report;
  const initialState = {
    from: "",
    to: "",
    mainElement: "",
    whatToShow: "",
    result: [],
  };
  const [model, setModel] = useState(initialState);

  let mainElements = [];
  mainElements.push({
    text: LabelNames.Users,
    key: LabelNames.Users,
    value: LabelNames.Users,
  });
  mainElements.push({
    text: LabelNames.News,
    key: LabelNames.News,
    value: LabelNames.News,
  });

  let whatToShowElement = [];
  whatToShowElement.push({
    text: LabelNames.Views,
    key: LabelNames.Views,
    value: LabelNames.Views,
  });
  whatToShowElement.push({
    text: LabelNames.Saved,
    key: LabelNames.Saved,
    value: LabelNames.Saved,
  });
  whatToShowElement.push({
    text: LabelNames.Reactions,
    key: LabelNames.Reactions,
    value: LabelNames.Reactions,
  });

  const handleFormsubmit = async (data) => {
    var result = await getReportModel(data);
    setModel(result);
  };
  const validationSchema = yup.object({});
  function reset() {
    setModel(initialState);
  }

  return (
    <React.Fragment>
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={model}
        onSubmit={(values) => handleFormsubmit(values)}
      >
        {({ handleSubmit }) => (
          <Form
            className="ui form"
            onSubmit={handleSubmit}
            style={{ backgroundColor: Colors.ShadeGray }}
          >
            <Grid>
              <Grid.Row style={{ backgroundColor: Colors.ShadeGray }}>
                <Grid.Column width={3}>
                  <MyDateInput name="from" placeholderText="from ..." />
                </Grid.Column>
                <Grid.Column width={3}>
                  <MyDateInput name="to" placeholderText="to ..." />
                </Grid.Column>
                <Grid.Column width={3}>
                  <MySelectInput
                    name="mainElement"
                    options={mainElements}
                    placehlder="with what to filter"
                  />
                </Grid.Column>
                <Grid.Column width={3}>
                  <MySelectInput
                    name="whatToShow"
                    options={whatToShowElement}
                    placehlder="what to show"
                  />
                </Grid.Column>

                <Grid.Column width={3}>
                  <Button
                    floated="right"
                    positive
                    type="submit"
                    content={ButtonNames.Submit}
                  />
                  <Button
                    onClick={() => reset()}
                    floated="right"
                    content={ButtonNames.Reset}
                  />
                </Grid.Column>
              </Grid.Row>
            </Grid>
          </Form>
        )}
      </Formik>

      {model.result.length == 0 &&
        model.mainElement != "" &&
        LabelNames.NothingFoundForFilter}

      {model.result.length > 0 && (
        <Table celled>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Id</Table.HeaderCell>
              <Table.HeaderCell>
                {model.mainElement == LabelNames.Users
                  ? LabelNames.name
                  : LabelNames.title}
              </Table.HeaderCell>
              <Table.HeaderCell>Number of</Table.HeaderCell>
            </Table.Row>
          </Table.Header>

          <Table.Body>
            {model.result.map((c) => (
              <Table.Row key={c.id}>
                <Table.Cell>
                  <Label ribbon>{c.id}</Label>
                </Table.Cell>
                <Table.Cell>{c.name}</Table.Cell>
                <Table.Cell>{c.number}</Table.Cell>
              </Table.Row>
            ))}
          </Table.Body>

          <Table.Footer>
            <Table.Row></Table.Row>
          </Table.Footer>
        </Table>
      )}
    </React.Fragment>
  );
}
