import React from 'react'
import { Button, Icon, Image, Modal } from 'semantic-ui-react'
import FormCustom from '../form/form'


function ModalExampleScrollingContent (props) {
  const [open, setOpen] = React.useState(false)
  console.log(props);

  return (
    <Modal
      open={open}
      onClose={() => setOpen(false)}
      onOpen={() => setOpen(true)}
      trigger={<Button>Создать задачу</Button>}
      style = {{height: '90%', opacity: '100'}}
    >
      <Modal.Header>Задача</Modal.Header>
      <Modal.Content image scrolling>
         
        <Modal.Description>
          <FormCustom props={props}/>
        </Modal.Description>
      </Modal.Content>
      <Modal.Actions>
        
      </Modal.Actions>
    </Modal>
  )
}

export default ModalExampleScrollingContent
