import React from "react";
import * as FaIcons from "react-icons/fa";
import RoutesConstants from "../../Constants/routes";
import LabelNames from "../../Constants/label-names";

export const SidebarData = [
  {
    title: LabelNames.Dashboard,
    path: RoutesConstants.HomePageRoute,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
  {
    title: LabelNames.Categories,
    path: RoutesConstants.Category,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
  {
    title: LabelNames.News,
    path: RoutesConstants.News,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
  {
    title: LabelNames.Configurations,
    path: RoutesConstants.Config,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
  {
    title: LabelNames.Admins,
    path: RoutesConstants.Users,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
  {
    title: LabelNames.Views,
    path: RoutesConstants.Views,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
  {
    title: LabelNames.Reactions,
    path: RoutesConstants.Reaction,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
  {
    title: LabelNames.Reports,
    path: RoutesConstants.Report,
    icon: <FaIcons.FaPlus />,
    cName: "nav-text",
  },
];
