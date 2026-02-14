import React, { useState } from "react";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement,
} from "chart.js";
import { Bar, Pie } from "react-chartjs-2";
import { useEffect } from "react";
import { Loading } from "../Loader/Loader";
import Days from "../../Constants/days";
import LabelNames from "../../Constants/label-names";
import Colors from "../../Constants/color";
import DashboardMainFilter from "../../Constants/dashboard-main-filter";
import { useStore } from "../Store";

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement
);

export const options = {
  responsive: true,
  plugins: {
    legend: {
      position: "top",
    },
    title: {
      display: true,
      text: LabelNames.Reactions,
    },
  },
};

export default function Dashboard() {
  const [selectedOption, setSelectedOption] = useState(
    +DashboardMainFilter.Week
  );
  const { Report } = useStore();
  const { GetReportView } = Report;
  const days = [
    Days.Monday,
    Days.Tuesday,
    Days.Wednesday,
    Days.Thursday,
    Days.Friday,
    Days.Saterday,
    Days.Sunday,
  ];
  const [result, setResult] = useState({
    views: [10, 3, 5, 6, 5, 1, 2],
    news: [10, 10, 10, 10, 10, 10, 10],
    users: [10, 10, 10, 10, 10, 10, 10],
    admins: 0,
    saved: 0,
    sad: 0,
    happy: 0,
    angry: 0,
  });
  const [labels, setLabels] = useState(days);
  const data = {
    labels,
    datasets: [
      {
        label: LabelNames.News,
        data: labels.map((item, index) => result.news[index]),
        backgroundColor: Colors.ShadePink,
      },
      {
        label: LabelNames.Views,
        data: labels.map((item, index) => result.views[index]),
        backgroundColor: Colors.LightSkyBlue,
      },
    ],
  };
  useEffect(() => {
    getData(DashboardMainFilter.Week);
  }, []);

  let filters = [];
  filters.push({
    text: LabelNames.Weeks,
    key: LabelNames.Weeks,
    value: DashboardMainFilter.Week,
  });
  filters.push({
    text: LabelNames.Mounths,
    key: LabelNames.Mounths,
    value: DashboardMainFilter.Mounth,
  });
  filters.push({
    text: LabelNames.Years,
    key: LabelNames.Years,
    value: DashboardMainFilter.Year,
  });

  const handleFormsubmit = (event) => {
    setSelectedOption(event.target.value);
    getData(event.target.value);

    setResult({
      views: [10, 3, 5, 6, 5, 1, 2],
      news: [1, 2, 30, 0, 10, 2, 10],
      users: [2, 13, 10, 1, 10, 0, 50],
      admins: 0,
      saved: 0,
      sad: 0,
      happy: 0,
      angry: 0,
    });
  };

  async function getData(id) {
    var model = await GetReportView(id);
    if (id == DashboardMainFilter.Week) {
      setLabels(days);
    } else if (id == DashboardMainFilter.Mounth) {
      setLabels([
        LabelNames.FirstWeek,
        LabelNames.SecondWeek,
        LabelNames.ThirdWeek,
        LabelNames.FourthWeek,
      ]);
    } else {
      setLabels([
        LabelNames.January,
        LabelNames.Febuary,
        LabelNames.March,
        LabelNames.April,
        LabelNames.May,
        LabelNames.June,
        LabelNames.July,
        LabelNames.August,
        LabelNames.September,
        LabelNames.October,
        LabelNames.November,
        LabelNames.December,
      ]);
    }
    setResult(model);
  }
  const piedata = {
    labels: [LabelNames.Users, LabelNames.Admins, LabelNames.Saved],
    datasets: [
      {
        label: "#",
        data: [result.users, result.admins, result.saved],
        backgroundColor: [
          Colors.ShadePink,
          Colors.LightSkyBlue,
          Colors.ShadeYellow,
        ],
        borderColor: [
          Colors.ShadePink,
          Colors.LightSkyBlue,
          Colors.ShadeYellow,
        ],
        borderWidth: 1,
      },
    ],
  };

  const piedata2 = {
    labels: [LabelNames.Angry, LabelNames.sad, LabelNames.happy],
    datasets: [
      {
        label: "#",
        data: [result.angry, result.sad, result.happy],
        backgroundColor: [
          Colors.ShadePink,
          Colors.LightSkyBlue,
          Colors.ShadeYellow,
        ],
        borderColor: [
          Colors.ShadePink,
          Colors.LightSkyBlue,
          Colors.ShadeYellow,
        ],
        borderWidth: 1,
      },
    ],
  };

  if (result == null) return <Loading />;
  return (
    <React.Fragment>
      <div className="row d-flex flex-row alig-items-center justify-content-center mt-3">
        <div className="col-md-3">{LabelNames.ShowOutputFor}</div>
        <div className="col-md-4">
          <select
            className="my-dropdown col-md-12"
            style={{ height: "25px" }}
            name="filter"
            value={selectedOption}
            onChange={handleFormsubmit}
          >
            <option value="1">{LabelNames.Weeks}</option>
            <option value="2">{LabelNames.Mounths}</option>
            <option value="3">{LabelNames.Years}</option>
          </select>
        </div>
      </div>
      <Bar options={options} data={data} />
      <div className="col-md-12 d-flex flex-row align-items-center justify-content-center">
        <div className="col-md-5 ">
          <Pie data={piedata} />
        </div>
        <div className="col-md-5 ">
          <Pie data={piedata2} />
        </div>
      </div>
    </React.Fragment>
  );
}
