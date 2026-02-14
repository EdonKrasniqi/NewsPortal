import { observer } from "mobx-react-lite";
import React, { useState } from "react";
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
import { useEffect } from "react";
import { Link } from "react-router-dom";
import AdminForm from "./AddAdmin";
import { Loading } from "../Loader/Loader";
import { useNavigate } from "react-router-dom";
import RoutesConstants from "../../Constants/routes";
import ButtonNames from "../../Constants/button-names";
import Colors from "../../Constants/color";
import genericFilter from "../../Functions/functions";

export default observer(function AdminList() {
  const { userStore } = useStore();
  const { users, selectUser, deleteUser } = userStore;
  const [open, setOpen] = React.useState(false);
  const [editmode, seteditmode] = useState();
  const [search, setSearch] = React.useState("");
  const navigate = useNavigate();

  useEffect(() => {
    userStore.loadUser();
    console.log(users);
  }, [userStore]);

  function ConfirmDelete(id) {
    userStore.selectUser(id);
    setOpen(true);
  }

  function afterconfirm() {
    deleteUser(userStore.selectedUserId);
    setOpen(false);
  }

  function editUser(id) {
    selectUser(id);
    seteditmode(true);
    navigate(RoutesConstants.AddAdmin);
  }

  if (users == null) return <Loading />;
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
            <Button
              color={Colors.blue}
              floated="right"
              as={Link}
              to={RoutesConstants.AddAdmin}
              content={<Icon name={ButtonNames.Add} />}
            />
          </Table.Row>

          <Table.Row>
            <Table.HeaderCell>Id</Table.HeaderCell>
            <Table.HeaderCell>Name</Table.HeaderCell>
            <Table.HeaderCell>Email</Table.HeaderCell>
            <Table.HeaderCell>Role</Table.HeaderCell>
            <Table.HeaderCell>Edit</Table.HeaderCell>
            <Table.HeaderCell>Delete</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {users
            .map((item) => (
              <Table.Row key={item.userId}>
                <Table.Cell>
                  <Label ribbon>{item.userId}</Label>
                </Table.Cell>
                <Table.Cell>
                  <Label>{item.userName}</Label>
                </Table.Cell>
                <Table.Cell>{item.email}</Table.Cell>
                <Table.Cell>{item.role}</Table.Cell>
                <Table.Cell>
                  <Button
                    color={Colors.blue}
                    onClick={() => editUser(item.userId)}
                    content={<Icon name={ButtonNames.Edit} />}
                  />
                </Table.Cell>
                <Table.Cell>
                  <Button color={Colors.red}
                    onClick={() => ConfirmDelete(item.userId)}>
                    <Icon name={ButtonNames.Delete} />
                  </Button>
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
      <Modal
        basic
        onClose={() => setOpen(false)}
        onOpen={() => setOpen(true)}
        open={open}
        size="small"
      >
        <Header icon>
          <Icon name={ButtonNames.Archive} />
          Delete User
        </Header>
        <Modal.Content>
          <p>Do you really want to delete this User</p>
        </Modal.Content>
        <Modal.Actions>
          <Button basic color={Colors.red} inverted onClick={() => setOpen(false)}>
            <Icon name={ButtonNames.Remove}/> No
          </Button>
          <Button color={Colors.green} inverted onClick={() => afterconfirm()}>
            <Icon name={ButtonNames.Checkmark} /> Yes
          </Button>
        </Modal.Actions>
      </Modal>

      <Modal
        basic
        onClose={() => seteditmode(false)}
        onOpen={() => seteditmode(true)}
        open={editmode}
      >
        <AdminForm />
      </Modal>
    </React.Fragment>
  );
});
