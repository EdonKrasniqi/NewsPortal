import React, { useEffect, useState } from "react";
import { Button, Label, Card, Image, Grid } from "semantic-ui-react";
import { Formik, Form } from "formik";
import * as yup from "yup";
import MyCheckBox from "../../FormElements/MyCheckBox";
import { Loading } from "../Loader/Loader";
import { useNavigate } from "react-router-dom";
import  ButtonNames  from "../../Constants/button-names";
import  LabelNames  from "../../Constants/label-names";
import { useStore } from "../Store";

export const NewsConfig = () => {
  const { Cofigurations } = useStore();
  const { loadConfigurations, configuration, updateConfigurations } = Cofigurations;
  const [configurationsModel, setConfiguration] = useState({
    newsConfigId: 1,
    headerLogo: "",
    footerLogo: "",
    showFeatured: true,
    showMostWatched: true,
    showRelatedNews: true,
  });

  useEffect(() => {
    loadConfigurations();
    setConfiguration(configuration[0]);
  }, []);

  const [headerImage, setHeaderImage] = useState();
  const [footerImage, setFooterImage] = useState();
  const [feature, setFeature] = useState(false);
  const [releted, setReleted] = useState(false);
  const [mostwatched, setMostWatched] = useState(false);
  const navigate = useNavigate();

  const changeFile = (event, name) => {
    let v = event.target.files;
    let reader = new FileReader();
    reader.readAsDataURL(v[0]);
    reader.onload = (e) => {
      setConfiguration({ ...configurationsModel, headerLogo: e.target?.result });
    };
  };

  const handleFormsubmit = async (Config) => {
    Config.headerImage = headerImage;
    Config.footerImage = footerImage;
    Config.showFeatured = feature;
    Config.showMostWatched = mostwatched;
    Config.showRelatedNews = releted;

    var result = await updateConfigurations(Config);
  };

  const validationSchema = yup.object({});
  function Close() {
    navigate("/");
  }

  if (configurationsModel == null) return <Loading />;

  return (
    <React.Fragment>
      {configuration != null && (
        <Formik
          validationSchema={validationSchema}
          enableReinitialize
          initialValues={configuration}
          onSubmit={(values) => handleFormsubmit(values)}
        >
          {({ handleSubmit }) => (
            <Form className="ui form" onSubmit={handleSubmit}>
              <h4 class="ui dividing header">Configuration</h4>
              <Grid>
                <Grid.Row>
                  <Card>
                    <Image src={configurationsModel.headerLogo} wrapped ui={false} />
                    <Card.Content>{LabelNames.HeaderImage}</Card.Content>
                    <Card.Content extra>
                      <input
                        type="file"
                        hidden
                        style={{ marginTop: "200px" }}
                        id="header"
                        onChange={(e) => changeFile(e, "headerLogo")}
                      />
                      <label
                        for="header"
                        className="btn"
                        style={{ marginTop: "10px" }}
                      >
                        <Label content={LabelNames.UploadImage} color="blue" />
                      </label>
                    </Card.Content>
                  </Card>
                </Grid.Row>
                <Grid.Row>
                  <Card>
                    <Image src={configurationsModel.footerLogo} wrapped ui={false} />
                    <Card.Content>{LabelNames.FooterLabel}</Card.Content>
                    <Card.Content extra>
                      <input
                        type="file"
                        hidden
                        style={{ marginTop: "200px" }}
                        id="footer"
                        onChange={(e) => changeFile(e, "footerLogo")}
                      />
                      <label
                        for="footer"
                        className="btn"
                        style={{ marginTop: "10px" }}
                      >
                        <Label content={LabelNames.UploadImage} color="blue" />
                      </label>
                    </Card.Content>
                  </Card>
                </Grid.Row>
                <Grid.Row>
                  <MyCheckBox
                    checked={feature}
                    onClick={() => setFeature(!feature)}
                    name="showFeatured"
                    label={LabelNames.ShowFeatured}
                  />
                </Grid.Row>
                <Grid.Row>
                  <MyCheckBox
                    checked={mostwatched}
                    onClick={() => setMostWatched(!mostwatched)}
                    name="showMostWatchd"
                    label={LabelNames.ShowMostViewed}
                  />
                </Grid.Row>
                <Grid.Row>
                  <MyCheckBox
                    checked={releted}
                    onClick={() => setReleted(!releted)}
                    name="showRelatedNews"
                    label={LabelNames.ShowRelatedNews}
                  />
                </Grid.Row>
              </Grid>

              <Button
                floated="right"
                positive
                type="subimit"
                content={ButtonNames.Submit}
              />
              <Button
                floated="right"
                onClick={() => Close()}
                content={ButtonNames.Cancel}
              />
            </Form>
          )}
        </Formik>
      )}
    </React.Fragment>
  );
};
