import { observer } from "mobx-react-lite";
import React from "react";
import { Icon, Label, Menu, Table } from "semantic-ui-react";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import Reaction from "../../enums/reaction-enum";
import LabelNames from "../../Constants/label-names";
import { useStore } from "../Store";

export default observer(function NewsReaction() {
  const { newsStore } = useStore();
  const [model, setModel] = useState();
  const { id } = useParams();

  useEffect(() => {
   loadData();
  }, []);
 async function loadData(){
   var data = await newsStore.getReactionForNews(id);
      setModel(data);
  }
  if (model == null) return <Loading />;
  return (
    <React.Fragment>
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>News Id</Table.HeaderCell>
            <Table.HeaderCell>News Title</Table.HeaderCell>
            <Table.HeaderCell>User</Table.HeaderCell>
            <Table.HeaderCell>Reaction</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {model != null &&
            model.map((item) => (
              <Table.Row key={item.newsId}>
                <Table.Cell>
                  <Label ribbon>{item.newsId}</Label>
                </Table.Cell>
                <Table.Cell>{item.title}</Table.Cell>
                <Table.Cell>{item.userName}</Table.Cell>
                <Table.Cell>
                  {item.reactionType == Reaction.Angry
                    ? LabelNames.Angry
                    : item.reactionType == Reaction.Sad
                    ? LabelNames.sad
                    : LabelNames.happy}
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
