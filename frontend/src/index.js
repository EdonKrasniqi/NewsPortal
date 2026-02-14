import React from 'react';
//import ReactDOM from 'react-dom/client';
import ReactDOM from 'react-dom'
import './index.css';
import App from './App';
import 'bootstrap/dist/css/bootstrap.min.css';
import reportWebVitals from './reportWebVitals';
import { store, StoreContext } from './Components/Store';
import { useNavigate } from 'react-router-dom';


export const history = useNavigate;
//const root = ReactDOM.createRoot(document.getElementById('root'));
ReactDOM.render(
  <StoreContext.Provider value={store}>
    <App />
    </StoreContext.Provider>,
    document.getElementById('root')

);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
