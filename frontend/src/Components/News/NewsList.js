import { observer } from "mobx-react-lite";
import React from "react";
import {
  Button,
  Icon,
  Label,
  Menu,
  Table,
  Modal,
  Header,
} from "semantic-ui-react";
import { useStore } from "../Store";
import  genericFilter  from "../../Functions/functions";
import { useEffect } from "react";
import NewsDetails from "./NewsDetails";
import { Link } from "react-router-dom";
import { Loading } from "../Loader/Loader";
import { useNavigate } from "react-router-dom";
import ButtonNames from "../../Constants/button-names";
import LabelNames from "../../Constants/label-names";
import Colors from "../../Constants/color";
import RoutesConstants from "../../Constants/routes";

export default observer(function NewsList() {
  const { newsStore } = useStore();
  const { news, selectNews, openDetails, closeDetails, detailsmode } =
    newsStore;
  const [open, setOpen] = React.useState(false);
  const [search, setSearch] = React.useState("");
  const navigate = useNavigate();

  useEffect(() => {
    newsStore.loadNews();
  }, [newsStore]);

  function openDetailsMode(id) {
    selectNews(id);
  }
  async function uploadNews(event) {
    try {
      let v = event.target.files;
      let file = v[0];
      await uploadNews(file);
    } catch (error) {}
  }

  function openForm() {
    navigate(RoutesConstants.AddNews);
  }

  async function excelExport() {
    await newsStore.excelExport(LabelNames.News);
  }

  if (news == null) return <Loading />;

  return (
    <React.Fragment>
      <Table celled>
        <Table.Header>
          <Table.Row>
            <div className="d-flex align-items-center justify-content-center">
              <div className="ui left icon input s">
                <input
                  type="text"
                  placeholder="Search ... "
                  onChange={(event) => setSearch(event.target.value)}
                />
                <i aria-hidden="true" className="users icon"></i>
              </div>
              <Button
                color={Colors.blue}
                className="ms-2"
                content={<Icon name={ButtonNames.Add} />}
                float="right"
                onClick={() => openForm()}
              />
              <Button
                color="green"
                content={<Icon name={ButtonNames.FileExcel} />}
                onClick={() => excelExport()}
              />
              <div className="col-md-3">
                <input type="file" hidden id="file" onChange={uploadNews} />
                <label for="file" className="btn">
                  <Label content={LabelNames.UploadImage} color={Colors.blue} />
                </label>
              </div>
            </div>
          </Table.Row>
          <Table.Row>
            <Table.HeaderCell>Id</Table.HeaderCell>
            <Table.HeaderCell>Category</Table.HeaderCell>
            <Table.HeaderCell>Tittle</Table.HeaderCell>
            <Table.HeaderCell>Details</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {genericFilter(news, (x) =>
            x.title.toLocaleLowerCase().includes(search.toLocaleLowerCase())
          ).map((c) => (
            <Table.Row key={c.id}>
              <Table.Cell>
                <Label ribbon>{c.id}</Label>
              </Table.Cell>
              <Table.Cell>{c.categoryId}</Table.Cell>
              <Table.Cell>{c.title}</Table.Cell>
              <Table.Cell>
                <Button
                  color="blue"
                  content={ButtonNames.Details}
                  onClick={() => openDetailsMode(c.id)}
                  as={Link}
                  to={RoutesConstants.NewsDetailsRoute(c.id)}
                />
              </Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>

        <Table.Footer>
          <Table.Row>
            <Table.HeaderCell colSpan="4">
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
      <Modal
        basic
        onClose={() => setOpen(false)}
        onOpen={() => setOpen(true)}
        open={open}
        size="small"
      >
        <Header icon>
          <Icon name={ButtonNames.Archive} />
          Delete News
        </Header>
        <Modal.Content>
          <p>Do you really want to delete this News?</p>
        </Modal.Content>
        <Modal.Actions>
          <Button basic color="red" inverted onClick={() => setOpen(false)}>
            <Icon name="remove" /> No
          </Button>
          <Button color="green" inverted onClick={() => setOpen(false)}>
            <Icon name="checkmark" /> Yes
          </Button>
        </Modal.Actions>
      </Modal>
      <Modal
        onClose={() => closeDetails()}
        onOpen={() => openDetails()}
        open={detailsmode}
      >
        <NewsDetails />
      </Modal>
    </React.Fragment>
  );
});
