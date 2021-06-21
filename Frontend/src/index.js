import React, { Component } from 'react';
import ReactDOM from 'react-dom';

import { Router } from "react-router-dom"
import {createBrowserHistory} from 'history'
import 'semantic-ui-less/semantic.less'
import './card-list.css'
import App from "./App";
import { CookiesProvider } from 'react-cookie';

const history = createBrowserHistory()

ReactDOM.render((
  <CookiesProvider><Router history={history}><App /></Router></CookiesProvider>),
   document.getElementById('root'))