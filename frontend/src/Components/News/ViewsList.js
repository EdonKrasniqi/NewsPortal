import React, { useState } from "react";
import { Button, Icon, Menu, Table } from "semantic-ui-react";
import { useEffect } from "react";
import { Loading } from "../Loader/Loader";
import { useNavigate } from "react-router-dom";
import { useStore } from "../Store";
import  ButtonNames  from "../../Constants/button-names";
import  Colors  from "../../Constants/color";
import  genericFilter  from "../../Functions/functions";

export default function ViewsList() {
  const { newsStore } = useStore();
  const [views, setViews] = useState();
  const [search, setSearch] = React.useState("");
  const navigate = useNavigate();

  useEffect(() => {
    newsStore.getViews().then((c) => {
      setViews(c.data);
    });
  }, []);
  function sendtoDetails(id) {
    navigate(`${id}`);
  }

  if (views == null) return <Loading />;

  return (
    <React.Fragment>
      <Table celled>
        <Table.Header>
          <Table.Row>
            <div className="ui left icon input s">
              <input
                type="text"
                placeholder="Search ... "
                onChange={(event) => setSearch(event.target.value)}
              />
              <i aria-hidden="true" className="users icon"></i>
            </div>
          </Table.Row>
          <Table.Row>
            <Table.HeaderCell>News Title</Table.HeaderCell>
            <Table.HeaderCell>Number of Clicks</Table.HeaderCell>
            <Table.HeaderCell>See details</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {genericFilter(views, (x) =>
            x.title.toLocaleLowerCase().includes(search.toLocaleLowerCase())
          ).map((v) => (
            <Table.Row key={v.newsId}>
              <Table.Cell>{v.title}</Table.Cell>
              <Table.Cell>{v.numberOfViews}</Table.Cell>
              <Table.Cell>
                <Button
                  onClick={() => sendtoDetails(v.newsId)}
                  content={ButtonNames.seeMore}
                  color={Colors.blue}
                />
              </Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>

        <Table.Footer>
          <Table.Row>
            <Table.HeaderCell colSpan="6">
              <Menu floated="right" pagination>
                <Menu.Item as="a" icon>
                  <Icon name="chevron left" />
                </Menu.Item>
                <Menu.Item as="a">1</Menu.Item>
                <Menu.Item as="a">2</Menu.Item>
                <Menu.Item as="a">3</Menu.Item>
                <Menu.Item as="a">4</Menu.Item>
                <Menu.Item as="a" icon>
                  <Icon name="chevron right" />
                </Menu.Item>
              </Menu>
            </Table.HeaderCell>
          </Table.Row>
        </Table.Footer>
      </Table>
    </React.Fragment>
  );
}
