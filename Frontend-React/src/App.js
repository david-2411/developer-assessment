import './App.css'
import React, { useState, useEffect } from 'react'
import TodoListPage from './pages/TodoList/TodoListPage'
import Footer from './components/Footer'

const App = () => {
  return (
    <div className="App">
      <TodoListPage/>
      <Footer/>
    </div>
  )
}

export default App
