import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { useStore } from "../Store";
import { Card, Button, Row, Container } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import { Icon } from "semantic-ui-react";
import  ButtonNames  from "../../Constants/button-names";
import  RoutesConstants  from "../../Constants/routes";

export default observer(function Saved() {
  const { newsStore, userStore } = useStore();
  const { addView, getSavedNews } = newsStore;
  const [news, setNews] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    getSavedNewsData();
  }, []);

  async function getSavedNewsData() {
   var result =await getSavedNews();
   setNews(result);
  }
  function remove(id) {
    newsStore.removeSavedNews(id, "");
    getSavedNewsData();
  }

  function sendToDetails(id) {
    addView(id, userStore.isLogged);
    navigate(RoutesConstants.NewsDetailsRoute(id));
  }

  if (news == null) return <Loading />;
  return (
    <React.Fragment>
      <Container className="mt-3">
        <Row>
          {news != null &&
            news.map((n) => (
              <Card
                key={n.newsId}
                style={{ width: "18rem", marginLeft: "10px" }}
              >
                <Card.Img variant="top" src={n.image} />
                <Card.Body style={{ cursor: "pointer" }}>
                  <Card.Text style={{ height: "100px", overflow: " hidden" }}>
                    {n.title}
                  </Card.Text>

                  <Button
                    onClick={() => sendToDetails(n.newsId)}
                    variant="primary"
                  >
                    ReadMore
                  </Button>
                  <Button
                    style={{ marginLeft: "20px" }}
                    onClick={() => remove(n.newsId)}
                    variant="danger"
                  >
                    <Icon name={ButtonNames.Remove} />
                  </Button>
                </Card.Body>
              </Card>
            ))}
        </Row>
      </Container>
    </React.Fragment>
  );
});
