import "./App.css";
import CategoryList from "./Components/Category/CategoriesList";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import NewsList from "./Components/News/NewsList";
import { React, Fragment } from "react";
import { observer } from "mobx-react-lite";
import { Container } from "semantic-ui-react";
import NewsDetails from "./Components/News/NewsDetails";
import NewsForm from "./Components/News/NewsForm";
import AddNews from "./Components/News/AddNews";
import { NewsConfig } from "./Components/Configuration/NewsConfiguration";
import Navbar from "./Components/Navbar/Navbar";
import { Register } from "./Components/Account/Register";
import Login from "./Components/Account/Login";
import Hompage from "./Components/Header/Hompage";
import Menu from "./Components/Header/Menu";
import NewsByCategory from "./Components/News/NewsByCategory";
import AdminsList from "./Components/Account/AdminsList";
import AdminForm from "./Components/Account/AddAdmin";
import { history } from "./index";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";
import "react-toastify/dist/ReactToastify.css";
import { toast, ToastContainer } from "react-toastify";
import axios from "axios";
import RoutesConstants from './Constants/routes'
import LocalStorageKey from './Constants/local-storage-keys'
import ErrorMessages from './Constants/error-messages'
import {useStore} from './Components/Store'
import Colors from './Constants/color'
import Dashboard from './Components/Dashboard/Dashboard'
import ViewList from  './Components/News/ViewsList'
import NewsViewsList from './Components/News/Views'
import Saved from './Components/News/Saved'
import ReactionDashboard from './Components/Reactions/ReactionDashboard'
import NewsReaction from './Components/Reactions/NewsReaction'
import NewsByTag from './Components/News/NewsByTag'
import FirstRaport from './Components/Raport/FirstRaport'
import EditProfile from './Components/Account/EditProfile'
import ResetPassword from './Components/Account/ResetPassword'
import ForgotPassword from './Components/Account/ForgotPassword'
import Footer from './Components/Footer/footer'

function App() {
  axios.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem(LocalStorageKey.TokenKey)}`;
    return config;
  });

  axios.interceptors.response.use(
    async (response) => {
      return response;
    },
    (error) => {
      const { data, status, } = error.response;
      switch (status) {
        case 400:
          if (data != null) {
            toast.error(data);
          } else {
            toast.error(ErrorMessages.SomethingWentWrong);
          }
          break;
        case 401:
          toast.error(ErrorMessages.Unauthorised);
          break;
        case 404:
          toast.error(ErrorMessages.NotFounded);
          break;
        case 500:
          toast.error(ErrorMessages.SomethingWentWrong);
          break;
      }
      return Promise.reject(error);
    }
  );
  const { userStore, newsStore } = useStore();
  newsStore.loadConfigs();


  document.body.style.backgroundColor = Colors.ShadeGray;

  if (localStorage.getItem(LocalStorageKey.TokenKey)) {
    userStore.getCurrentUser();
  }
  return (
    <Fragment>
    <Router history={history}>
      {userStore.isAdmin ? (
        <Container style={{ backgroundColor: Colors.ShadeGray }}>
          <Navbar />
          <Routes>
            <Route path={RoutesConstants.Hompage} element={<Dashboard />} />
            <Route path={RoutesConstants.Category} element={<CategoryList />} />
            <Route path={RoutesConstants.News} element={<NewsList />} />
            <Route path={RoutesConstants.NewsDetails} element={<NewsDetails />} />
            <Route path={RoutesConstants.EditNews} element={<NewsForm />} />
            <Route path={RoutesConstants.AddNews} element={<AddNews />} />
            <Route path={RoutesConstants.Config} element={<NewsConfig />} />
            <Route path={RoutesConstants.Users} element={<AdminsList />} />
            <Route path={RoutesConstants.AddAdmin} element={<AdminForm />} />
            <Route path={RoutesConstants.Views} element={<ViewList />} />
            <Route path={RoutesConstants.ViewsDetails} element={<NewsViewsList />} />
            <Route path={RoutesConstants.Saved} element={<Saved />} />
            <Route path={RoutesConstants.Reaction} element={<ReactionDashboard />} />
            <Route path={RoutesConstants.ReactionDetails} element={<NewsReaction />} />
            <Route path={RoutesConstants.NewsByTag} element={<NewsByTag />} />
            <Route path={RoutesConstants.Report} element={<FirstRaport />} />
            <Route path={RoutesConstants.EditProfile} element={<EditProfile />} />
            <Route path={RoutesConstants.WildCard} element={<Dashboard />} />
          </Routes>
          <ToastContainer />
        </Container>
      ) : (
        <div style={{ backgroundColor: Colors.ShadeGray }}>
          <Menu style={{ marginBottom: "100px" }} />
          <div style={{ minHeight: "60%", height: "100%" }}>
            <Routes>
              <Route path={RoutesConstants.Hompage} element={<Hompage />} />
              <Route path={RoutesConstants.Login} element={<Login />} />
              <Route path={RoutesConstants.NewsByCategory} element={<NewsByCategory />} />
              <Route path={RoutesConstants.NewsDetails} element={<NewsDetails />} />
              <Route path={RoutesConstants.Register} element={<Register />} />
              <Route path={RoutesConstants.Saved} element={<Saved />} />
              <Route path={RoutesConstants.NewsByTag} element={<NewsByTag />} />
              <Route path={RoutesConstants.ResetPassword} element={<ResetPassword />} />
              <Route path={RoutesConstants.ForgotPassword} element={<ForgotPassword />} />
              <Route path={RoutesConstants.WildCard} element={<Hompage />} />
            </Routes>
          </div>
          <ToastContainer />
          <Footer style={{ marginTop: "100px" }} />
        </div>
      )}
    </Router>
  </Fragment>
  );
}

export default observer(App);
