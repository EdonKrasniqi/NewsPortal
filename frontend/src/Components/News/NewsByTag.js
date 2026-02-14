import { observer } from "mobx-react-lite";
import React, { useEffect, useState } from "react";
import { useStore } from "../Store";
import { Card, Button, Row, Container } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import RoutesConstants from "../../Constants/routes";
import LabelNames from "../../Constants/label-names";

export default observer(function NewsbyTag() {
  const navigate = useNavigate();
  const { newsStore, userStore } = useStore();
  const { getNewsByTags } = newsStore;
  const { id } = useParams();

  const { addView } = newsStore;
  const [news, setNews] = useState();

  useEffect(() => {
    getNewsByTags(id).then((c) => {
      setNews(c.data);
    });
  }, [id]);

  function afterClick(newsId) {
    addView(newsId, userStore.isLogged);
    navigate(RoutesConstants.NewsDetailsRoute(newsId));
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
                style={{
                  width: "18rem",
                  marginLeft: "10px",
                  marginTop: "10px",
                }}
              >
                <Card.Img variant="top" src={`${n.image}`} />
                <Card.Body>
                  <Card.Text>{n.title}</Card.Text>
                  <Button
                    onClick={() => afterClick(n.id)}
                    variant="primary"
                  >
                    {LabelNames.ReadMore}
                  </Button>
                </Card.Body>
              </Card>
            ))}
        </Row>
      </Container>
    </React.Fragment>
  );
});
