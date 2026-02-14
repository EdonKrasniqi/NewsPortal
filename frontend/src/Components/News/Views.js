import React, { useState } from "react";
import {
  Icon,
  Menu,
  Table,
} from "semantic-ui-react";
import { useStore } from "../Store";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import { Loading } from "../Loader/Loader";

export default function NewsViewsList() {
  const { newsStore } = useStore();
  const {getViewsForNews } = newsStore;
  const [views, setViews] = useState();
  const { id } = useParams();

  useEffect(() => {
    getViewsForNews(id).then((c) => {
      setViews(c.data);
    });
  }, []);

  if (views == null) return <Loading />;

  return (
    <React.Fragment>
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>User Id</Table.HeaderCell>
            <Table.HeaderCell>User Name</Table.HeaderCell>
            <Table.HeaderCell>FingerPrintID</Table.HeaderCell>
            <Table.HeaderCell>time</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {views != null &&
            views.map((v) => (
              <Table.Row>
                <Table.Cell>{v.userId}</Table.Cell>
                <Table.Cell>{v.userName}</Table.Cell>
                <Table.Cell>{v.fingerPrintId}</Table.Cell>
                <Table.Cell>{v.viewedOn}</Table.Cell>
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
