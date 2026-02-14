import { makeAutoObservable, runInAction } from "mobx";
import axios from "axios";

export default class CategoryStore {
  baseUrl = "https://localhost:44306/Category";
  selectedCategory = undefined;
  editmode = false;
  detailsmode = false;
  categoriesRegistry = new Map();
  selectedCategoryId = 0;

  constructor() {
    makeAutoObservable(this);
  }

  loadCategories = async () => {
      const categories = await (
        await axios.get(this.baseUrl)
      ).data;
      runInAction(() => {
        categories.forEach((x) => {
          this.categoriesRegistry.set(x.categoryId, x);
        });
      });
  };

  get categories() {
    return Array.from(this.categoriesRegistry.values());
  }

  selectCategory = (id) => {
    this.selectedCategoryId = id;
    this.selectedCategory = this.categoriesRegistry.get(id);
  };

  canceleSelectedCategory = () => {
    this.selectedCategoryId = 0;
    this.selectedCategory = undefined;
  };

  openForm = (id) => {
    id ? this.selectCategory(id) : this.canceleSelectedCategory();
    this.editmode = true;
  };

  openDetails = (id) => {
    this.selectCategory(id);
    this.detailsmode = true;
  };
  
  closeDetails = () => {
    this.selectedCategory = undefined;
    this.detailsmode = false;
  };

  closeForm = () => {
    this.editmode = false;
  };

  createCategory = async (category) => {
      var id = await (
        await axios.post(this.baseUrl, category)
      ).data;
      runInAction(() => {
        category.categoryId = id;
        this.categoriesRegistry.set(id, category);
        this.selectCategory(category);
        this.editmode = false;
      });
    }

  updateCategory = async (category) => {
      await axios.post(this.baseUrl, category);
      runInAction(() => {
        this.categoriesRegistry.set(category.categoryId, category);
        this.selectCategory(category);
        this.editmode = false;
      });
  };
  
  deleteCategory = async (id) => {
      await axios.delete(`${this.baseUrl}/${id}`);
      runInAction(() => {
        this.categoriesRegistry.delete(id);
        this.canceleSelectedCategory();
      });
  }
}

