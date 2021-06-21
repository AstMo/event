import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import { Link } from "react-router-dom"


import "./chart-statistics"

class Statistic extends Component {

  render() {

    return (
      <div>
        <canvas id="myChart3"></canvas>
        <script src="https://cdn.jsdelivr.net/npm/chart.js@2.8.0"></script>
      </div>



    );

  }
}


export default Statistic
