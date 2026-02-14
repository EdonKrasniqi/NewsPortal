import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { useStore } from "../Store";
import sadPhoto from "../UserView/sad.png";
import happyPhoto from "../UserView/sad.png";
import angryPhoto from "../UserView/angry.png";
import { Loading } from "../Loader/Loader";
import {
  Container,
  Grid,
  Image,
  Modal,
  Header,
  Icon,
  Segment,
  Rail,
} from "semantic-ui-react";
import { Button } from "react-bootstrap";
import { useParams } from "react-router-dom";
import "../UserView/Carosel.css";
import { useNavigate, Link } from "react-router-dom";
import  ReactionTypes  from "../../Constants/reaction-type";
import  RoutesConstants from '../../Constants/routes'
import  LabelNames  from "../../Constants/label-names";
import  Colors  from "../../Constants/color";

export default observer(function NewsDetails() {
  const { newsStore, userStore } = useStore();
  const [item, setItem] = useState();
  const [model, setOpenModel] = useState(false);
  const { news, addView, loadNews, getNewsDetails, getReactionForNews, saveNews } =
    newsStore;
  const [sad, setSad] = useState(0);
  const [happy, setHappy] = useState(0);
  const [angry, setAngry] = useState(0);
  const navigate = useNavigate();
  const { id } = useParams();
  const [isSaved, setIsSaved] = useState(false);

  useEffect(() => {
    loadNews();
    LoadData()
    getReactions();
  }, [id]);

  async function LoadData(){
    var data = await  getNewsDetails(id);
    setItem(data);
    setIsSaved(data.isSaved);
  }

  async function getReactions() {
   let reactions = await getReactionForNews(id);
      setSad(reactions.filter((x) => x.reactionType == ReactionTypes.Sad).length);
      setHappy(
        reactions.filter((x) => x.reactionType == ReactionTypes.Happy).length
      );
      setAngry(
        reactions.filter((x) => x.reactionType == ReactionTypes.Angry).length
      );
  }

  function Delete(id) {
    setOpenModel(true);
  }

  async function ConfirmDelete() {
      var url = await newsStore.deleteNews(newsStore.selectedNewsId);
      navigate(url);
  }

  async function addReaction(nr) {
      if (!userStore.isLogged) {
        navigate(RoutesConstants.Login);
      } else {
       await newsStore.addReaction(item.id,nr);
        getReactions();
      }
  }

  const settings = {
    className: "center",
    infinite: true,
    centerPadding: "60px",
    slidesToShow: 5,
    swipeToSlide: true,
    afterChange: function (index) {
      
    },
  };

  function sendToEdit(id) {
    navigate(RoutesConstants.AddNewsRoute(id));
  }

  async function sendToDetails(newsId) {
    await addView(newsId, userStore.isLogged);
    navigate(RoutesConstants.NewsDetailsRoute(newsId));
  }

  async function  Save(id){
      if (!userStore.isLogged) {
        navigate(RoutesConstants.Login);
      } else {
        await saveNews(id)
        setIsSaved(true);
      }
  }

  if (item == null) return <Loading />;

  return (
    <React.Fragment>
      <Container>
        <Grid>
          <Grid.Row centered>
            <Grid.Column Row={5} style={{ wordBreak: "break-all" }}>
              <Image src={`${item.image}`} fluid />
            </Grid.Column>
          </Grid.Row>
          <Grid.Row centered>
            <h1
              style={{
                color:Colors.DarkShadeOfGray,
                fontWeight: "bold",
                whiteSpace: "break-spaces",
              }}
            >
              {item != null && item.title}
            </h1>
          </Grid.Row>
          <Grid.Row centered>
            <h3 style={{ color: Colors.DarkShadeOfGray, fontWeight: "bold" }}>
              {item != null && item.subTitle}
            </h3>
          </Grid.Row>
          <Grid.Row>
            <p style={{ color: Colors.DarkShadeOfGray }}>{item != null && item.content}</p>
          </Grid.Row>
          <Grid.Row></Grid.Row>
          <Grid.Row>
            {item != null && (
              <div dangerouslySetInnerHTML={{ __html: item.video }}></div>
            )}
          </Grid.Row>
          <Grid.Row>
            {item != null &&
              item.tags != null &&
              item.tags
                .split(",")
                .filter((x) => x != "")
                .map((c) => (
                  <React.Fragment>
                    <Grid.Column fluid>
                      <Link to={RoutesConstants.NewsByTagRoute(c)}>
                        <Button
                          href={RoutesConstants.NewsByTagRoute(c)}
                          className="btn btn-primary btn-lg "
                        >
                          {c}
                        </Button>
                      </Link>
                    </Grid.Column>
                    <Grid.Column style={{ width: "15px" }}></Grid.Column>
                  </React.Fragment>
                ))}
          </Grid.Row>
          <Grid.Row style={{ marginTop: "10px" }}>
            <Grid.Column fluid>
              <Button color={Colors.blue} className="btn-primary btn-lg mr-20">
                <Icon
                  onClick={() => Save(item.id)}
                  color={isSaved ? Colors.red : ""}
                  name="heart"
                />
              </Button>
            </Grid.Column>
            <Grid.Column fluid>
              {item != null && userStore.isAdmin && (
                <Button
                  color={Colors.blue}
                  onClick={() => sendToEdit(item.id)}
                  className="btn-primary btn-lg"
                >
                  Edit
                </Button>
              )}
            </Grid.Column>
            <Grid.Column fluid>
              {item != null && userStore.isAdmin && (
                <Button
                  color={Colors.red}
                  onClick={() => Delete(item.id)}
                  className="btn-danger mr-20 btn-lg"
                >
                  Delete
                </Button>
              )}
            </Grid.Column>
          </Grid.Row>
          <Grid.Row>
            <h3>{LabelNames.HowAreYouFeelingAboutThis}</h3>
          </Grid.Row>
          <Grid.Row>
            <Grid.Column width={2}>
              <Segment>
                <Image
                  style={{ cursor: "pointer" }}
                  src={sadPhoto}
                  width="100px"
                  height="100px"
                  onClick={() => addReaction(+ReactionTypes.Sad)}
                />
                <Rail
                  internal
                  position="right"
                  style={{ width: "10px", marginRight: "50px" }}
                >
                  <div style={{ float: "right" }}>{sad}</div>
                </Rail>
              </Segment>
            </Grid.Column>
            <Grid.Column width={2}>
              <Segment>
                <Image
                  style={{ cursor: "pointer" }}
                  src={happyPhoto}
                  width="100px"
                  height="100px"
                  onClick={() => addReaction(+ReactionTypes.Happy)}
                />
                <Rail
                  internal
                  position="right"
                  style={{ width: "10px", marginRight: "50px" }}
                >
                  <div style={{ float: "right" }}>{happy}</div>
                </Rail>
              </Segment>
            </Grid.Column>
            <Grid.Column width={2}>
              <Segment>
                <Image
                  style={{ cursor: "pointer" }}
                  src={angryPhoto}
                  width="100px"
                  height="100px"
                  onClick={() => addReaction(+ReactionTypes.Angry)}
                />
                <Rail
                  internal
                  position="right"
                  style={{ width: "10px", marginRight: "50px" }}
                >
                  <div style={{ float: "right" }}>{angry}</div>
                </Rail>
              </Segment>
            </Grid.Column>
          </Grid.Row>
        </Grid>
        <div style={{ marginTop: "50px" }}>
          <h2> Related News</h2>
        </div>
      </Container>
      <Modal
        basic
        onClose={() => setOpenModel(false)}
        onOpen={() => setOpenModel(true)}
        open={model}
        size="small"
      >
        <Header icon>
          <Icon name="archive" />
          Delete News
        </Header>
        <Modal.Content>
          <p>Do you really want to delete this News?</p>
        </Modal.Content>
        <Modal.Actions>
          <Button
            basic
            color="red"
            className="btn-sucess ml-20"
            inverted
            onClick={() => setOpenModel(false)}
          >
            <Icon name="remove" /> {LabelNames.No}
          </Button>
          <Button
            color="green"
            className="btn-danger mr-20"
            inverted
            onClick={() => ConfirmDelete()}
          >
            <Icon name="checkmark" /> {LabelNames.Yes}
          </Button>
        </Modal.Actions>
      </Modal>
    </React.Fragment>
  );
});
