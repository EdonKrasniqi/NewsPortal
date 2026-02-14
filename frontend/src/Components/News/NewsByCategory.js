import { observer } from "mobx-react-lite";
import React, { useEffect } from "react";
import { useStore } from "../Store";
import { Card, Row, Container } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import { Link } from "react-router-dom";
import  RoutesConstants  from "../../Constants/routes";

export default observer(function NewsbyCategory() {
  const { newsStore, userStore } = useStore();
  const { id } = useParams();
  const { isLogged } = userStore;

  const { news, addView } = newsStore;

  useEffect(() => {
    newsStore.loadNews();
  }, [newsStore, id]);

  if (news == null) return <Loading />;
  return (
    <React.Fragment>
      <Container className="mt-3">
        <Row>
          {news
            .filter((x) => x.categoryId == id)
            .map((n) => (
              <Card
                onClick={() => addView(n.newsId, isLogged)}
                key={n.newsId}
                style={{
                  width: "18rem",
                  marginLeft: "10px",
                  marginTop: "10px",
                }}
              >
                <Link to={RoutesConstants.NewsDetailsRoute(n.id)}>
                  <Card.Img variant="top" src={`${n.image}`} />
                  <Card.Body>
                    <Card.Text>{n.title}</Card.Text>
                  </Card.Body>
                </Link>
              </Card>
            ))}
        </Row>
      </Container>
    </React.Fragment>
  );
});
