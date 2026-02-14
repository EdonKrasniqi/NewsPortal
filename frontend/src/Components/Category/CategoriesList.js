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
import { useEffect } from "react";
import { CategoryForm } from "./CategoryForm";
import { Loading } from "../Loader/Loader";
import ButtonNames  from "../../Constants/button-names";
import  genericFilter from '../../Functions/functions'


export default observer(function CategoryList() {
  const { categoryStore } = useStore();
  const {
    categories,
    deleteCategory,
    selectCategory,
    openForm,
    closeForm,
    editmode,
  } = categoryStore;
  const [open, setOpen] = React.useState(false);
  const [search, setSearch] = React.useState("");

  useEffect(() => {
    categoryStore.loadCategories();
  }, [categoryStore]);

  function ConfirmDelete(id) {
    selectCategory(id);
    setOpen(true);
  }

  function afterconfirm() {
    deleteCategory(categoryStore.selectedCategoryId);
    setOpen(false);
  }

  if (categories == null) return <Loading />;

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
              color="blue"
              content={<Icon name="add" />}
              float="right"
              onClick={() => openForm()}
            />
          </Table.Row>
          <Table.Row>
            <Table.HeaderCell>Id</Table.HeaderCell>
            <Table.HeaderCell>Name</Table.HeaderCell>
            <Table.HeaderCell>Description</Table.HeaderCell>
            <Table.HeaderCell>ShowOnline</Table.HeaderCell>
            <Table.HeaderCell>Edit</Table.HeaderCell>
            <Table.HeaderCell>Delete</Table.HeaderCell>
          </Table.Row>
        </Table.Header>

        <Table.Body>
          {genericFilter(categories, x=> x.name
                  .toLocaleLowerCase()
                  .includes(search.toLocaleLowerCase()))
            .map((item) => (
              <Table.Row key={item.categoryId}>
                <Table.Cell>
                  <Label ribbon>{item.categoryId}</Label>
                </Table.Cell>
                <Table.Cell>{item.name}</Table.Cell>
                <Table.Cell>{item.description}</Table.Cell>
                <Table.Cell>{item.showOnline ? "Yes" : "No"}</Table.Cell>
                <Table.Cell>
                  <Button
                    color="blue"
                    content={<Icon name={ButtonNames.Edit} />}
                    onClick={() => openForm(item.categoryId)}
                  />
                </Table.Cell>
                <Table.Cell>
                  <Button
                    color="red"
                    onClick={() => ConfirmDelete(item.categoryId)}
                  >
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
          Delete Category
        </Header>
        <Modal.Content>
          <p>Do you really want to delete this Category?</p>
        </Modal.Content>
        <Modal.Actions>
          <Button basic color="red" inverted onClick={() => setOpen(false)}>
            <Icon name={ButtonNames.Remove} /> No
          </Button>
          <Button color="green" inverted onClick={() => afterconfirm()}>
            <Icon name={ButtonNames.Checkmark} /> Yes
          </Button>
        </Modal.Actions>
      </Modal>
      <Modal
        onClose={() => closeForm(false)}
        onOpen={() => openForm(true)}
        open={editmode}
      >
        <CategoryForm />
      </Modal>
    </React.Fragment>
  );
});
