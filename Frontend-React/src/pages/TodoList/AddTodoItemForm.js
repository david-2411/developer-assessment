import { Button, Container, Row, Col, Form, Stack } from 'react-bootstrap'
import React, { useState, useId } from 'react'
import { apiUrl } from '../../data/constants'
import axios from "axios";

function AddTodoItemForm ({ refetchItems }) {
    const [description, setDescription] = useState('')
    const handleDescriptionChange = (event) => {
      setDescription(event.target.value)
    }

    async function handleAdd() {
      if(description === "") {
        alert("Description can't be empty")
        return;
      }
      try {
        var payload = {
          id: useId,
          description: description,
          isCompleted: false
        }
        const response = await axios.post(apiUrl+"TodoItems", payload)
        console.log(response)
        setDescription("");
        refetchItems();        
      } catch (error) {
        alert(error.response.data)
      }
    }
  
    function handleClear() {
      setDescription('')
    }

    return (
      <Container>
        <h1>Add Item</h1>
        <Form.Group as={Row} className="mb-3" controlId="formAddTodoItem">
          <Form.Label column sm="2">
            Description
          </Form.Label>
          <Col md="6">
            <Form.Control
              type="text"
              placeholder="Enter description..."
              value={description}
              onChange={handleDescriptionChange}
            />
          </Col>
        </Form.Group>
        <Form.Group as={Row} className="mb-3 offset-md-2" controlId="formAddTodoItem">
          <Stack direction="horizontal" gap={2}>
            <Button variant="primary" onClick={() => handleAdd()}>
              Add Item
            </Button>
            <Button variant="secondary" onClick={() => handleClear()}>
              Clear
            </Button>
          </Stack>
        </Form.Group>
      </Container>
    )
}

export default AddTodoItemForm;