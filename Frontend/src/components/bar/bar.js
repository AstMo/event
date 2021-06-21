import React, { Component, useState, useEffect } from 'react';
import { Link } from "react-router-dom"

import { HomeOutlined, UserOutlined } from '@ant-design/icons';
import {
  Button,
  Checkbox,
  Grid,
  Header,
  Icon,
  Image,
  Input,
  Menu,
  Segment,
  Sidebar
} from "semantic-ui-react";
import './bar.css';

import Avatar from '../upload/upload'



function Bar(props) {

  console.log(props);

  const [visible, setVisible] = useState(false)

  function show() {
    setVisible(!visible)
  }



  return (

    <div className='bar'>
      <Link className='logo' to="/events">
        <img src='logo.png' className='logo' />
      </Link>

      <HomeOutlined style={{ color: '#171738', fontSize: '24px', zIndex: '999' }} className='btnHome' />

      <UserOutlined style={{ color: '#171738', fontSize: '24px', zIndex: '999' }} className='btnPerson' onClick={show} />
      {/* <SidebarCustom visible={visible} setVisible={setVisible}/> */}
      <Sidebar.Pushable style={{ position: 'absolute', right: '0', zIndex: '1000', top: '50px', height: 'calc(100vh - 50px)'}}>
        <Sidebar
          as={Menu}
          animation="overlay"
          icon="labeled"
          direction="right"
          onHide={() => setVisible(false)}
          vertical
          visible={visible}
          width="thin"
          style={{ backgroundColor: "white", width: "300px" }}
        >
          <Menu.Item
            style={{
              display: "flex",
              justifyContent: "center",
              flexDirection: "row"
            }}

          >
             <h1>{props.user.username}</h1>
          </Menu.Item>
          <Menu.Item
            style={{
              display: "flex",
              justifyContent: "center",
              flexDirection: "row"
            }}

          >
             <Avatar />
          </Menu.Item>

          
            <Menu.Item as="a">
              <Input placeholder="ФИО" />
            </Menu.Item>
            <Menu.Item as="a">
              <Input placeholder="Дата рождения" />
            </Menu.Item>
            <Menu.Item as="a">
              <Input placeholder="Адрес" />
            </Menu.Item>
            <Menu.Item as="a" >
              <Input placeholder="О себе" />
            </Menu.Item>
            <Menu.Item as="a">
              <Button>Сохранить</Button>
            </Menu.Item>
  
        </Sidebar>
          <Sidebar.Pusher>
            <Segment basic style={{ width: '500px', height: '500px' }}>

            </Segment>
          </Sidebar.Pusher>

      </Sidebar.Pushable>



    </div>
  )

}



export default Bar;