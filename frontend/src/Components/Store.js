import { createContext,useContext } from "react";
import CategoryStore from "../Components/Category/CategoryStore";
import UserStore from "../Components/Account/UserStore";
import NewsStore from "../Components/News/NewsStore";
import ConfigurationStore from "../Components/Configuration/SiteConfiguration";
import ReportStore from "../Components/Dashboard/ReportConfiguration"

export const store = {
    categoryStore: new CategoryStore(),
    newsStore:new NewsStore(),
    userStore:new UserStore(),
    Cofigurations: new ConfigurationStore(),
    Report: new ReportStore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);
}

