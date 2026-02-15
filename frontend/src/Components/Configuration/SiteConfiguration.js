import axios from "axios";
import { makeAutoObservable, runInAction } from "mobx";

class ConfigurationStore {
  baseUrl = "https://localhost:44335/Config";
  configurations = new Map();

  constructor() {
    makeAutoObservable(this);
  }

  loadConfigurations = async () => {
    const result = await axios.get(this.baseUrl);
    runInAction(() => {
        this.configurations.set(result.data.id, result.data);
    });
  };
  get configuration() {
    return Array.from(this.configurations.values());
  }

  updateConfigurations = async (model)=>{
   await axios.post(this.baseUrl, model);
  }
}

export default ConfigurationStore;
