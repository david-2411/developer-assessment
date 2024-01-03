import { Button, Table } from 'react-bootstrap'
import React, { useState } from 'react'
import { apiUrl } from '../../data/constants'
import axios from "axios";

function TodoItemsTable ({ refetchItems, items }) {

    async function getItems() {
        try {
          refetchItems();
        } catch (error) {
          console.error(error)
        }
    }
    
    async function handleMarkAsComplete(item) {
        try {
            const payload = {
              id: item.id,
              description: item.description,
              isCompleted: true
            }
            const response = await axios.put(apiUrl+"TodoItems/"+item.id, payload);
            console.log(response)
            getItems();
          } catch (error) {
            alert(error.response.data)
          }
    }

    return (
      <>
        <h1>
          Showing {items.length} Item(s){' '}
          <Button variant="primary" className="pull-right" onClick={() => getItems()}>
            Refresh
          </Button>
        </h1>

        <Table striped bordered hover>
          <thead>
            <tr>
              <th>Id</th>
              <th>Description</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.description}</td>
                <td>
                  <Button variant="warning" size="sm" onClick={() => handleMarkAsComplete(item)}>
                    Mark as completed
                  </Button>
                </td>
              </tr>
            ))}
          </tbody>
        </Table>
      </>
    )
}

export default TodoItemsTable