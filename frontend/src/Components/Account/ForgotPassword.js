import React, { useState } from "react";
import styless from "./styless.css";
import { useStore } from "../Store";
import LabelNames  from "../../Constants/label-names";

export const ForgotPassword = () => {
  const [email, setEmail] = useState("");
  const [data, setData] = useState();
  const [isError, setIsError] = useState();

  const { userStore } = useStore();
  const { forgotPassword } = userStore;

  async function restPassword() {
    let item = { email };
     forgotPassword(item).then(res => {
      setIsError(false);
      setData("A reset link has been sent!")
      setEmail('');
    })
      .catch(err => {
        setIsError(true);
        setData(err.response.statusText);
      });
  }

  return (
    <div className="wraper">

      <div className="container">
        <div className="textbox">
          <h3>{LabelNames.ResetYourPassword}</h3>
          <p className="paragrafff">Lost your password? Please enter your email address. <br />You will receive a link to create a new password via email.</p>
        </div>
        <div className="col-sm-4 ">
          <input
            type="text"
            placeholder="Email"
            onChange={(e) => setEmail(e.target.value)}
            className="form-control"
          />
          <br />
          <button onClick={restPassword} className="btn btn-primary">
            Reset
          </button>
          <br />
          <br />
          {data && (
            <div className='form-group'>
              <div
                className={isError ? 'alert alert-danger' : 'alert alert-success'}
                role='alert'
              >
                {data}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ForgotPassword;
