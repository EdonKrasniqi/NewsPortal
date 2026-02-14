import axios from "axios";
import { makeAutoObservable, runInAction } from "mobx";
import Roles from "../../Constants/roles";
import LocalStorageKey from "../../Constants/local-storage-keys";

class UserStore {
  baseUrl = "https://localhost:44306/Users";
  accountBaseUrl = "https://localhost:44306/Account";
  selectedUserId = 0;
  selectedUser = undefined;
  user = {};
  userName = "";
  userEmail = "";
  currentUserId = "";
  userRole = "";
  isAdmin = false;
  isLogged = false;
  UserRegistry = new Map();

  constructor() {
    makeAutoObservable(this);
  }

  loadUser = async () => {
    const result = await axios.get(this.baseUrl + "/GetUsers");
    const users = result.data;
    runInAction(() => {
      users.forEach((user) => {
        this.UserRegistry.set(user.userId, user);
      });
    });
  };
  get users() {
    return Array.from(this.UserRegistry.values());
  }

  usersById(id) {
    return this.UserRegistry.values().filter((x) => x.id == id);
  }
  selectUser = (id) => {
    this.selectedUserId = id;
    this.selectedUser = this.UserRegistry.get(id);
  };

  canceleSelectedUser = () => {
    this.selectedUserId = 0;
    this.selectedUser = undefined;
  };

  AddAdmin = async (user) => {
    await axios.post(`${this.accountBaseUrl + "/AddAdmin"}`, user);
    runInAction(() => {});
  };

  EditUser = async (user, file) => {
    const formData = new FormData();
    user.Id = user.userId;
    formData.append("file", file);
    for (const property in user) {
      formData.append(property, user[property]);
    }
    await axios.post(`${this.baseUrl + "/UpdateUser"}`, formData);
    runInAction(() => {});
  };

  deleteUser = async (id) => {
    await axios.delete(`${this.baseUrl + "/DeleteUser"}`);
    runInAction(() => {
      this.UserRegistry.delete(id);
      this.canceleSelectedUser();
    });
  };

  forgotPassword = (item) => {
    return axios.post(`${this.accountBaseUrl}/forgot-password`, item);
  };

  resetPassword = (item) => {
    return axios.post(`${this.accountBaseUrl}/reset-password`, item);
  };

  getCurrentUser = async () => {
    const user = await (await axios.get(this.accountBaseUrl)).data;
    runInAction(() => {
      this.setUsersProperties(user);
    });
  };

  login = async (model) => {
    const fpPromise = import("https://openfpcdn.io/fingerprintjs/v3").then(
      (FingerprintJS) => FingerprintJS.load()
    );
    fpPromise
      .then((fp) => fp.get())
      .then(async (result) => {
        const visitorId = result.visitorId;
        model.fingerPrintId = visitorId;
        const userResult = await (
          await axios.post(`${this.accountBaseUrl + "/Login"}`, model)
        );
        const user = userResult.data
        runInAction(() => {
          console.log(user);
          this.setUsersProperties(user);
        });
        return "/";
      });
  };

  setUsersProperties(user) {
    if (user != null) {
      this.userEmail = user.userEmail;
      this.userRole = user.userRole;
      this.userName = user.username;
      this.isLogged = true;
      this.currentUserId = user.id;
      this.isAdmin = user.userRole == Roles.Admin;
      window.localStorage.setItem(LocalStorageKey.TokenKey, user.token);
    } else {
      this.userEmail = "";
      this.userRole = "";
      this.currentUserId = "";
      this.userName = "";
      this.isLogged = false;
      this.isAdmin = false;
      window.localStorage.removeItem(LocalStorageKey.TokenKey);
    }
  }

  logout = () => {
    this.setUsersProperties(null);
  };

  addUser = async (User) => {
    var result = await (await axios.post(`${this.accountBaseUrl}`, User)).data;
    runInAction(() => {});
  };

  register = async (creds) => {
    var result = await (await axios.post(`${this.accountBaseUrl}`, creds)).data;
    runInAction(() => {
      return "/login";
    });
  };
}

export default UserStore;
