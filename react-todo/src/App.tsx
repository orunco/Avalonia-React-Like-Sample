import React, {useState} from "react";
import styled from 'styled-components';
import Header from './Header.tsx';
import TodoList from './TodoList.tsx';
import Footer from './Footer.tsx';

interface Todo {
    id: number;
    name: string;
    done: boolean;
}

const App: React.FC = () => {
    const [todos, setTodos] = useState<Todo[]>([
        {id: 1, name: '吃饭', done: true},
        {id: 2, name: '睡觉', done: true},
        {id: 3, name: '写代码', done: false},
        {id: 4, name: '逛街', done: true},
    ]);

    const handleAddNewTodo = (newName: string) => {
        const isNameUnique = !todos.some(todo => todo.name === newName);

        if (isNameUnique) {
            setTodos(prevTodos => [
                {
                    id: Math.max(...prevTodos.map(todo => todo.id), 0) + 1,
                    name: newName,
                    done: false
                },
                ...prevTodos,
            ]);
        } else {
            alert(`Todo with name "${newName}" already exists`);
        }
    };

    const changeDoneFlagCallBack = (id: number, done: boolean) => {
        const todoExists = todos.some(todo => todo.id === id);

        if (!todoExists) {
            console.warn(`Todo with ID ${id} not found`);
            return;
        }

        setTodos(prevTodos =>
            prevTodos.map(todo =>
                todo.id === id ? {...todo, done} : todo
            )
        );
    };

    const handleDeleteCallBack = (id: number) => {
        const todoExists = todos.some(todo => todo.id === id);

        if (!todoExists) {
            console.warn(`Todo with ID ${id} not found, cannot delete`);
            return;
        }

        setTodos(prevTodos => prevTodos.filter(todo => todo.id !== id));
    };

    const allDoneCallBack = (flag: boolean) => {
        setTodos(prevTodos =>
            prevTodos.map(todo => ({
                ...todo,
                done: flag
            }))
        );
    };

    const removeAllDoneCallBack = () => {
        setTodos(prevTodos => prevTodos.filter(todo => !todo.done));
    };

    return (
        <AppContainer className="todo-container">
            <div className="todo-wrap">
                <Header onEnter={handleAddNewTodo}/>
                <TodoList
                    todos={todos}
                    changeDoneFlagCallBack={changeDoneFlagCallBack}
                    handleDeleteCallBack={handleDeleteCallBack}
                />
                <Footer
                    todos={todos}
                    allDoneCallBack={allDoneCallBack}
                    removeAllDoneCallBack={removeAllDoneCallBack}
                />
            </div>
        </AppContainer>
    );
};

export const AppContainer = styled.div`
    .todo-wrap {
        padding: 10px;
        border: 1px solid #ddd;
        border-radius: 5px;
    }
`;

export default App;
