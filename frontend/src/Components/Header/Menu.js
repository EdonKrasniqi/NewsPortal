import { observer } from "mobx-react-lite";
import React from "react";
import { useStore } from "../Store";
import { Icon } from "semantic-ui-react";
import "./home.css";
import logo from "../../logo.svg";
import { Navbar, Container, Nav, Image, Button } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import  RoutesConstants  from "../../Constants/routes";
import  Colors  from "../../Constants/color";
import  LabelNames  from "../../Constants/label-names";

export default observer(function Menu() {
  const { categoryStore, userStore, newsStore } = useStore();
  const { categories } = categoryStore;
  const navigate = useNavigate();

  const logOut = () => {
    userStore.logout();
    navigate(RoutesConstants.HomePageRoute);
  };

  return (
    <Navbar
      style={{
        color: Colors.lightGray,
        backgroundColor: Colors.DarkShadeOfBlue,
      }}
      collapseOnSelect
      bg={Colors.HexDarkShadeOfBlue}
      variant="light"
    >
      <Container>
        <Link to={RoutesConstants.HomePageRoute}>
          {" "}
          <Navbar.Brand href={`${RoutesConstants.HomePageRoute}`}>
            {
              <Image
                src={newsStore.headerImg}
                height={"50px"}
                width={"200px"}
              />
            }
          </Navbar.Brand>
        </Link>
        <Navbar.Toggle aria-controls="responsive-navbar-nav" />
        <Navbar.Collapse id="responsive-navbar-nav">
          <Nav className="me-auto">
            <Link
              to={RoutesConstants.HomePageRoute}
              style={{ textDecoration: "none", color: Colors.WhiteColor }}
            >
              {" "}
              <Nav.Link
                href={RoutesConstants.HomePageRoute}
                style={{
                  textDecoration: "none",
                  color: Colors.WhiteColor,
                  cursor: "pointer",
                }}
              >
                <Image src={logo} rounded size={"small"} />
              </Nav.Link>
            </Link>
            {categories.map((c) => (
              <React.Fragment>
                <Link
                  style={{ textDecoration: "none", color: Colors.WhiteColor }}
                  to={RoutesConstants.CategoryDetailsRoute(c.id)}
                >
                  {" "}
                  <Nav.Link
                    style={{
                      textDecoration: "none",
                      color: Colors.WhiteColor,
                      cursor: "pointer",
                    }}
                    href={RoutesConstants.CategoryDetailsRoute(c.id)}
                  >
                    {c.name}
                  </Nav.Link>
                </Link>
              </React.Fragment>
            ))}
          </Nav>

          {!userStore.isLogged && (
            <Nav>
              <Link
                to={RoutesConstants.Login}
                style={{ textDecoration: "none", color: Colors.WhiteColor }}
              >
                <Nav.Link
                  style={{
                    textDecoration: "none",
                    color: Colors.WhiteColor,
                    cursor: "pointer",
                  }}
                  href={RoutesConstants.Login}
                >
                  <Icon name="user" /> {LabelNames.Login}
                </Nav.Link>
              </Link>
              <Link
                style={{ textDecoration: "none", color: Colors.WhiteColor  }}
                to={RoutesConstants.Register}
              >
                <Nav.Link
                  style={{
                    textDecoration: "none",
                    color: Colors.WhiteColor ,
                    cursor: "pointer",
                  }}
                  eventKey={2}
                  href={RoutesConstants.Register}
                >
                  <Icon name="user plus" />
                  {LabelNames.Register}
                </Nav.Link>
              </Link>
            </Nav>
          )}
          {userStore.isLogged && (
            <Nav>
              <Nav.Link
                style={{
                  textDecoration: "none",
                  color: Colors.WhiteColor,
                  cursor: "pointer",
                }}
                href=""
              >
                {userStore.userName}
              </Nav.Link>
              <Nav.Link
                style={{
                  textDecoration: "none",
                  color: Colors.WhiteColor,
                  cursor: "pointer",
                }}
                onClick={() => logOut()}
              >
                {LabelNames.logOut}
              </Nav.Link>
              <Link
                style={{ textDecoration: "none", color: Colors.WhiteColor }}
                to={RoutesConstants.Saved}
              >
                {" "}
                <Nav.Link
                  style={{
                    textDecoration: "none",
                    color: Colors.WhiteColor,
                    cursor: "pointer",
                  }}
                  href={RoutesConstants.Saved}
                >
                  <Button className="btn btn-primary">{LabelNames.Saved}</Button>
                </Nav.Link>
              </Link>
            </Nav>
          )}
        </Navbar.Collapse>
      </Container>
    </Navbar>
  );
});
