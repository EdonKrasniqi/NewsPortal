import axios from "axios";
import { makeAutoObservable, runInAction } from "mobx";

class ReportStore {
  baseUrl = "https://localhost:44306/Report";

  constructor() {
    makeAutoObservable(this);
  }

  GetReportView = async (filter) => {
  var result = await (
        await axios.get(`${this.baseUrl}/GetDashboarViewModel/${filter}`)
      ).data;
    runInAction(() => {
      });

      return result;
  };

  get configuration() {
    return Array.from(this.configurations.values()[0]);
  }

  getReportModel = async (model)=>{
   var result = await axios.post(this.baseUrl, model);
   return result.data;
  }
}

export default ReportStore;
