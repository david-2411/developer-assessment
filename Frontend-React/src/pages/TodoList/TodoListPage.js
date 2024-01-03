import { Image, Alert, Container, Row, Col } from 'react-bootstrap'
import TodoItemsTable from './TodoItemsTable'
import AddTodoItemForm from './AddTodoItemForm'
import { useEffect, useState } from 'react'
import useFetch from '../../hooks/useFetch'
import { apiUrl } from '../../data/constants'

function TodoListPage()
{
    const {data, loading, error, refetch } = useFetch(apiUrl + "TodoItems");
    const [items, setItems] = useState([])

    useEffect(()=>{
        if(data != null) {
            setItems(data)
        }
    },[data])

    useEffect(()=>{
        if(error != null)
        {
            alert(error)
        }
    })
    
    function refetchItems() {
        refetch();
    }
    return (
        <Container>
            <Row>
                <Col>
                    <Image src="clearPointLogo.png" fluid rounded />
                </Col>
            </Row>
            <Row>
                <Col>
                    <Alert variant="success">
                    <Alert.Heading>Todo List App</Alert.Heading>
                    Welcome to the ClearPoint frontend technical test. We like to keep things simple, yet clean so your
                    task(s) are as follows:
                    <br />
                    <br />
                    <ol className="list-left">
                        <li>Add the ability to add (POST) a Todo Item by calling the backend API</li>
                        <li>
                        Display (GET) all the current Todo Items in the below grid and display them in any order you wish
                        </li>
                        <li>
                        Bonus points for completing the 'Mark as completed' button code for allowing users to update and mark
                        a specific Todo Item as completed and for displaying any relevant validation errors/ messages from the
                        API in the UI
                        </li>
                        <li>Feel free to add unit tests and refactor the component(s) as best you see fit</li>
                    </ol>
                    </Alert>
                </Col>
            </Row>
            <Row>
                <Col><AddTodoItemForm refetchItems={refetchItems}/></Col>
            </Row>
            <br />
            <Row>
                <Col><TodoItemsTable refetchItems={refetchItems} items={items}/></Col>
            </Row>
        </Container>
    )
}

export default TodoListPage