export default class RoutesConstants {
  static HomePageRoute = "/";
  static Login = "/login";
  static Register = "/register";
  static ResetPassword = "/reset-password";
  static ForgetPassword = "/forgotpassword";

  static News = "/News";
  static NewsDetails = "/News/details/:id";
  static NewsByCategory = "/category/:id";
  static NewsByTag = "/tag/:id";
  static EditNews = "/News/add/:id";
  static AddNews = "/News/add";

  static Users = "/users";
  static AddAdmin = "/addAdmin";
  static EditProfile = "/edit-profile";

  static Saved = "/saved";

  static Config = "/Config";

  static Views = "/views";
  static ViewsDetails = "/views/:id";

  static Reaction = "/reaction";
  static ReactionDetails = "/reaction/:id";

  static Report = "/raports";

  static WildCard = "*";

  static NewsDetailsRoute = (id) => `/News/details/${id}`;
  static CategoryDetailsRoute = (id) => `/category/${id}`;
  static AddNewsRoute = (id) => `/News/add/${id}`;
  static NewsByTagRoute = (tag) => `/tag/${tag}`;
  static ReactionDetailsRoute = (id) => `reaction/${id}`;
}
