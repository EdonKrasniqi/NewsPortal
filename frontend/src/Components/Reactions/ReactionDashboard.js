import { observer } from "mobx-react-lite";
import React from "react";
import {
  Button,
  Icon,
  Label,
  Menu,
  Table,
} from "semantic-ui-react";
import { useEffect, useState } from "react";
import { Loading } from "../Loader/Loader";
import { useNavigate } from "react-router-dom";
import  ButtonNames  from "../../Constants/button-names";
import { useStore } from "../Store";
import  RoutesConstants  from "../../Constants/routes";

export default observer(function ReactionDashboard() {
  const { newsStore } = useStore();
  const [model, setModel] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    loadData()
  }, []);

 async function loadData(){
    var data = await newsStore.getReactions()
      setModel(data);
  }

  function SendTODetails(id) {
    navigate(id);
  }

  if (model == null) return <Loading />;

  return (
    <React.Fragment>
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Id</Table.HeaderCell>
            <Table.HeaderCell>News Title</Table.HeaderCell>
            <Table.HeaderCell>Sad</Table.HeaderCell>
            <Table.HeaderCell>Happy</Table.HeaderCell>
            <Table.HeaderCell>Angry</Table.HeaderCell>
            <Table.HeaderCell></Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {model.map((c) => (
                <Table.Row key={c.newsId}>
                  <Table.Cell>
                    <Label ribbon>{c.newsId}</Label>
                  </Table.Cell>
                  <Table.Cell>{c.title}</Table.Cell>
                  <Table.Cell>{c.sad}</Table.Cell>
                  <Table.Cell>{c.happy}</Table.Cell>
                  <Table.Cell>{c.angry}</Table.Cell>
                  <Table.Cell>
                    <Button
                      color="blue"
                      content={ButtonNames.More}
                      onClick={() => SendTODetails(c.newsId)}
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
});
