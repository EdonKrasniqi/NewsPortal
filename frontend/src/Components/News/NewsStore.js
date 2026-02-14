import { makeAutoObservable, runInAction } from "mobx";
import axios from "axios";

export default class NewsStore {
  newsbaseUrl = "https://localhost:44306/News";
  newsConfigBaseUrl = "https://localhost:44306/Config";
  userNewsBaseUrl = "https://localhost:44306/UserNews";
  selectedNewsId = undefined;
  headerImg = undefined;
  footerImg = undefined;
  showFeatured = undefined;
  showMostWached = undefined;
  showReleted = undefined;
  selectedNews = undefined;
  editmode = false;
  detailsmode = false;
  NewsRegistry = new Map();

  constructor() {
    makeAutoObservable(this);
  }

  loadNews = async () => {
    const news = await axios.get(`${this.newsbaseUrl}`);
    console.log(news.data);
    runInAction(() => {
      console.log(news.data);
      news.data.forEach((news) => {
        this.NewsRegistry.set(news.id, news);
      });
    });
  };

  get news() {
    return Array.from(this.NewsRegistry.values());
  }

  loadConfigs = async () => {
    const newsConfig = await (await axios.get(this.newsConfigBaseUrl)).data;

    runInAction(() => {
      this.headerImg = newsConfig.headerLogo;
      this.footerImg = newsConfig.footerLogo;
      this.showFeatured = newsConfig.showFeatured;
      this.showMostWached = newsConfig.showMostWatched;
      this.showReleted = newsConfig.showRelatedNews;
    });
  };

  selectNews = (id) => {
    this.selectedNewsId = id;
    this.selectedNews = this.NewsRegistry.get(id);
  };

  canceleSelectedNews = () => {
    this.selectedNews = undefined;
    this.selectedNewsId = 0;
  };

  openForm = (id) => {
    id ? this.selectNews(id) : this.canceleSelectedNews();
    this.editmode = true;
  };

  getNewsByTags = (tags) => {
    return axios.get(`${this.newsbaseUrl}/GetNewsByTag/${tags}`);
  };

  addView = (id, isLoggedIn) => {
    if (isLoggedIn) {
      axios.post(`${this.userNewsBaseUrl}/AddView`, {
        newsId: id,
        userId: "",
      });
    } else {
      const fpPromise = import("https://openfpcdn.io/fingerprintjs/v3").then(
        (FingerprintJS) => FingerprintJS.load()
      );
      fpPromise
        .then((fp) => fp.get())
        .then((result) => {
          const visitorId = result.visitorId;
          axios.post(`${this.userNewsBaseUrl}/AddView`, {
            newsId: id,
            FingerPrintId: visitorId,
          });
        });
    }
  };

  saveNews = (id) => {
    axios.post(`${this.userNewsBaseUrl}/AddSavedNews`, {
      newsId: id,
    });
  };

  openDetails = (id) => {
    this.selectNews(id);
    this.detailsmode = true;
  };

  closeDetails = () => {
    this.selectedNews = undefined;
    this.detailsmode = false;
  };

  closeForm = () => {
    this.editmode = false;
  };

  createNews = async (news, file) => {
    if (news.expireDate) {
      let date = news.expireDate;
      news.expireDate = `${date.getDate()}/${
        date.getMonth() + 1
      }/${date.getFullYear()}`;
    }
    const formData = new FormData();
    formData.append("file", file);
    for (const property in news) {
      formData.append(property, news[property]);
    }
    var id = await (
      await axios.post(`${this.newsbaseUrl}/AddOrEdit`, formData)
    ).data;
    runInAction(() => {
      news.id = id;
      this.NewsRegistry.set(id, news);
      this.selectNews(news);
      this.editmode = false;
    });
  };

  updateNews = async (news, file) => {
    const formData = new FormData();
    formData.append("file", file);
    for (const property in news) {
      formData.append(property, news[property]);
    }
    await axios.post(`${this.newsbaseUrl}/AddOrEdit`, formData);
    runInAction(() => {
      this.NewsRegistry.set(news.id, news);
      this.selectNews(news);
      this.editmode = false;
    });
  };

  deleteNews = async (id) => {
    await axios.delete(`${this.newsbaseUrl}/${id}`);
    runInAction(() => {
      this.NewsRegistry.delete(id);
      this.canceleSelectedNews();
    });
    return "/news";
  };

  getNewsDetails = async (id) => {
    var data = await axios.get(`${this.newsbaseUrl}/${id}`);
    return data.data;
  };
  addReaction = async (newsId, reactionType) => {
    var result = await axios.post(`${this.userNewsBaseUrl}/AddReaction`, {
      newsId: newsId,
      reactionType: reactionType,
    });

    return result;
  };

  getReactionForNews = async (id) => {
    var data = await axios.get(
      `${this.userNewsBaseUrl}/GetNewsReactions/${id}`
    );
    return data.data;
  };

  uploadNews = async (file) => {
    const formData = new FormData();
    formData.append("file", file);
    await axios.post(`${this.baseUrl}/UpdateAddNews`, formData);
  };

  excelExport = async (fileName) => {
    const excel = await await axios.get(`${this.newsbaseUrl}/GetExcel`, {
      responseType: "blob",
    });
    const blob = new Blob([excel.data]);
    const link = document.createElement("a");
    link.href = window.URL.createObjectURL(blob);
    link.download = `${fileName}.xlsx`;
    link.click();
  };

  getSavedNews = async () => {
    var data = await axios.get(`${this.userNewsBaseUrl}/GetSavedNews`);
    return data.data;
  };
  removeSavedNews = async (newsId, userId) => {
    let result = await axios.post(`${this.userNewsBaseUrl}/DeleteSavedNews`, {
      newsId: newsId,
      userId: userId,
    });
    return result;
  };
  getViewsForNews = async (id) => {
    return axios.get(`${this.userNewsBaseUrl}/GetViewForNews/${id}`);
  };

  getViews = async () => {
    var result = await axios.get(`${this.userNewsBaseUrl}/GetViews`);
    return result.data;
  };
  getReactions = async () => {
    var result = await axios.get(`${this.userNewsBaseUrl}/GetReactions`);
    return result.data;
  };
}
