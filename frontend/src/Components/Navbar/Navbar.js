import React, { useState } from "react";
import * as FaIcons from "react-icons/fa";
import * as AiIcons from "react-icons/ai";
import { Link } from "react-router-dom";
import { SidebarData } from "./Navbardata";
import "./Navbar.css";
import { IconContext } from "react-icons";
import { Menu, Dropdown, Icon } from "semantic-ui-react";
import { useStore } from "../Store";
import Colors from "../../Constants/color";
import RoutesConstants from "../../Constants/routes";
import LabelNames from "../../Constants/label-names";

function Navbar() {
  const { userStore } = useStore();
  const { user } = useStore;
  const [sidebar, setSidebar] = useState(false);

  const showSidebar = () => setSidebar(!sidebar);
  return (
    <>
      <IconContext.Provider value={{ color: Colors.WhiteColor }}>
        <div className="navbar">
          <Link to="#" className="menu-bars">
            <FaIcons.FaBars
              classNamt="ngjyra-sidebarhover"
              onClick={showSidebar}
            />
          </Link>

          <Menu.Item position="right">
            <Icon name="user" color="white" />
            <Dropdown
              style={{ color: "white", float: "left" }}
              pointing="top left"
              text={user?.userName ?? ''}
            >
              <Dropdown.Menu>
                <Dropdown.Item
                  as={Link}
                  to={RoutesConstants.EditProfile}
                  text={LabelNames.MyProfile}
                />
                <Dropdown.Item
                  as={Link}
                  to={RoutesConstants.Saved}
                  text={LabelNames.SavedNews}
                />
                <Dropdown.Item
                  text={LabelNames.logOut}
                  icon="power"
                  onClick={() => userStore.logout()}
                />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        </div>
        <nav className={sidebar ? "nav-menu active" : "nav-menu"}>
          <ul className="nav-menu-items" onClick={showSidebar}>
            <li className="navbar-toggle">
              <Link to="#" className="menu-bars">
                <AiIcons.AiOutlineClose />
              </Link>
            </li>
            {SidebarData.map((item, index) => {
              return (
                <li key={index} className={item.cName}>
                  <Link to={item.path}>
                    {item.icon}
                    <span>{item.title}</span>
                  </Link>
                </li>
              );
            })}
          </ul>
        </nav>
      </IconContext.Provider>
    </>
  );
}

export default Navbar;
