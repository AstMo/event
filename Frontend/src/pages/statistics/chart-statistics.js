import React from 'react';
import { Doughnut } from 'react-chartjs-2';
import './statistics'


const dataBudget = {
    labels: ['Купить платье невесты', 'Нанять дизайнера', 'Нанять ведущего', 'Купить костюм жениха', 'Нанять фотографа'],
    datasets: [
        {
            data: [60000, 40000, 30000, 20000, 19600],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
                'rgba(153, 102, 255, 0.2)',
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
                'rgba(153, 102, 255, 1)',
            ],
            borderWidth: 1,
        },
    ],
};

const dataTasks = {
    labels: ['В обсуждении', 'Сделать', 'В процессе', 'Выполнено'],
    datasets: [
        {
            data: [2, 2, 2, 6],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)',
                'rgba(54, 162, 235, 0.2)',
                'rgba(255, 206, 86, 0.2)',
                'rgba(75, 192, 192, 0.2)',
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)',
                'rgba(54, 162, 235, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(75, 192, 192, 1)',
            ],
            borderWidth: 1,
        },
    ],
};

const DoughnutChart = () => (
    <div className = 'charts'>
        <div className='header'>
            <h1 className='title'>Распределение бюджета</h1>
        </div>
        <Doughnut data={dataBudget} />
        <div className='header'>
            <h1 className='title'>Готовность задач</h1>
        </div>
        <Doughnut data={dataTasks} />
    </div>
);

export default DoughnutChart;
