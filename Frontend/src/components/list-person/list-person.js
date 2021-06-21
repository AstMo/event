import React, { useEffect, useState } from 'react'
import { Dropdown } from 'semantic-ui-react'

const friendOptions = [
  {
    key: 'Jenny Hess',
    text: 'Jenny Hess',
    value: 'Jenny Hess',
    image: { avatar: true, src: '/iconPerson.png' },
  },
  {
    key: 'Elliot Fu',
    text: 'Elliot Fu',
    value: 'Elliot Fu',
    image: { avatar: true, src: '/iconPerson.png' },
  },
  {
    key: 'Stevie Feliciano',
    text: 'Stevie Feliciano',
    value: 'Stevie Feliciano',
    image: { avatar: true, src: '/iconPerson.png' },
  },
  {
    key: 'Christian',
    text: 'Christian',
    value: 'Christian',
    image: { avatar: true, src: '/iconPerson.png' },
  },
  {
    key: 'Matt',
    text: 'Matt',
    value: 'Matt',
    image: { avatar: true, src: '/iconPerson.png' },
  },
  {
    key: 'Justen Kitsune',
    text: 'Justen Kitsune',
    value: 'Justen Kitsune',
    image: { avatar: true, src: '/iconPerson.png' },
  },
]

function DropdowSelection(props) {
  const [users, setUsers] = useState()
  let space = ' '
  console.log(props);

  useEffect(() => {
    (async () => {

      let response = await fetch(`http://cv-dentistry.ru/api/Account/Search/\s`, {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${props.token}`
        },

      })
      if (response.ok) {
        let res = await response.json()
        console.log(res);
        let arr = []
        res.result.items.map((user) => {
          arr.push({key: user.name, text: user.name, value: user.name, image: { avatar: true, src: '/iconPerson.png' }})
        })
        setUsers(arr)
      }
    })()
  }, [])
  if (users) {
    return (
      <Dropdown
        placeholder='Выбрать пользователей'
        fluid
        multiple selection
        options={users}
      />
    )
  } else {
    return (

      <div>123</div>
    )
  }
}

export default DropdowSelection



