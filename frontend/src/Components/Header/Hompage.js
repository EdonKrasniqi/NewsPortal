import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useStore } from "../Store";
import adds from "../UserView/adds.jpg";
import { Button, Card, Container, Carousel } from "react-bootstrap";
import { Item, Grid, Image } from "semantic-ui-react";
import Slider from "react-slick";
import { Loading } from "../Loader/Loader";
import { useNavigate } from "react-router-dom";
import LabelNames from "../../Constants/label-names";
import Colors from "../../Constants/color";
import RoutesConstants from "../../Constants/routes";

export default observer(function HomePage() {
  const { newsStore, categoryStore, userStore } = useStore();

  const { news, addView } = newsStore;
  const { categoriesRegistry } = categoryStore;
  const navigate = useNavigate();
  useEffect(() => {
    newsStore.loadNews();
  }, [newsStore]);
  const settings = {
    className: "center",
    infinite: true,
    centerPadding: "60px",
    slidesToShow: 5,
    swipeToSlide: true,
    afterChange: function (index) {},
  };

  function sendToDetails(id) {
    addView(id, userStore.isLogged);
    navigate(RoutesConstants.NewsDetailsRoute(id));
  }

  if (news == null) return <Loading />;

  return (
    <React.Fragment>
      <Container className="mt-3">
        <Grid>
          <Grid.Row>
            <Carousel style={{ border: "1px solid lightblack" }}>
              {news != null &&
                news.map((x) => (
                  <Carousel.Item
                    style={{ alignItems: "center" }}
                    onClick={() => sendToDetails(x.id)}
                  >
                    <img
                      className="d-block w-100"
                      src={x.image}
                      alt="First slide"
                    />
                    <Carousel.Caption style={{ alignItems: "center" }}>
                      <h2>{x.title}</h2>
                      <p>{x.subTitle}</p>
                    </Carousel.Caption>
                  </Carousel.Item>
                ))}
            </Carousel>
          </Grid.Row>

          <Grid.Row style={{ marginTop: "125px" }}>
            <h3>In Focus</h3>
          </Grid.Row>
          <Grid.Row>
            <Grid.Column width={9}>
              <Item.Group
                divided
                style={{
                  backgroundColor: Colors.WhiteColor,
                  border: "1px solid #eaeaea",
                }}
              >
                {news.map((n) => (
                  <Item onClick={() => sendToDetails(n.id)} size="small">
                    <Item.Image size="small" src={n.image} />

                    <Item.Content>
                      <Item.Description>
                        <p style={{ marginTop: "20px" }}></p>
                        <p style={{ textAlign: "start" }}>{n.title}</p>
                        <p style={{ color: Colors.red, textAlign: "start" }}>
                          {categoriesRegistry.get(n.categoryId)?.name}
                        </p>
                      </Item.Description>
                    </Item.Content>
                  </Item>
                ))}
              </Item.Group>
            </Grid.Column>
            <Grid.Column></Grid.Column>
            <Grid.Column width={3}>
              <Grid.Row>
                <Item.Group>
                  {news
                    .filter((x) => x.newsId > 38)
                    .map((n) => (
                      <Item onClick={() => sendToDetails(n.id)}>
                        <Item.Image size="tiny" src={n.image} />
                        <Item.Content verticalAlign="middle">
                          <Item.Header as="a">{n.title}</Item.Header>
                        </Item.Content>
                      </Item>
                    ))}
                </Item.Group>
              </Grid.Row>
              <Grid.Row style={{ marginTop: "150px" }}>
                <Image src={adds} />
              </Grid.Row>
            </Grid.Column>
          </Grid.Row>
          <Grid.Row>
            <h3>{LabelNames.MostViewed}</h3>
          </Grid.Row>
          <Grid.Row>
            <Slider {...settings}>
              {news != null &&
                news.map((n) => (
                  <Card
                    key={n.newsId}
                    style={{ width: "18rem", marginLeft: "10px" }}
                    size="medium"
                  >
                    <Card.Img variant="top" src={n.image} />
                    <Card.Body>
                      <Card.Text
                        style={{ height: "100px", overflow: " hidden" }}
                      >
                        {n.title}
                      </Card.Text>

                      <Button
                        onClick={() => sendToDetails(n.id)}
                        variant="primary"
                      >
                        {LabelNames.ReadMore}
                      </Button>
                    </Card.Body>
                  </Card>
                ))}
            </Slider>
          </Grid.Row>
        </Grid>
      </Container>
    </React.Fragment>
  );
});
