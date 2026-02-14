import React, { useState, useEffect } from "react";
import { useStore } from "../Store";
import styless from "./styless.css"
import {useLocation } from "react-router-dom";
import LabelNames from '../../Constants/label-names'

export const ResetPassword = () => {
  const { userStore } = useStore();
  const { resetPassword } = userStore;
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const location = useLocation();
  const [token, setToken] = useState("");
  const [data, setData] = useState();
  const [isError, setIsError] = useState();

  useEffect(() => { 
    const searchParams = new URLSearchParams(location.search);
    const myParam = searchParams.get('token');
     console.log(myParam);
    setToken(myParam.toString());
  },[]);
  
  async function reset() {
    let item = { email, password, token };
    let result = await resetPassword(item).then(res => {
      setIsError(false);
      setData("Password changed succesfully!")
      setEmail('');
      setPassword('');
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
          <p className="paragraf">Please enter your new password</p>
        </div>
        <div className="col-sm-4 ">
          <input
            type="text"
            placeholder="Email"
            onChange={(e) => setEmail(e.target.value)}
            className="form-control"
          />
          <br />
          <input
            type="password"
            placeholder="New Password"
            onChange={(e) => setPassword(e.target.value)}
            className="form-control"
          />
          <br />
          <button onClick={reset} className="btn btn-primary">
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

export default ResetPassword;
