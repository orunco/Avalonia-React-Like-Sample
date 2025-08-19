import React from "react";
import styled from 'styled-components';
import TodoItem from "./TodoItem.tsx";

interface Todo {
    id: number;
    name: string;
    done: boolean;
}

interface TodoListProps {
    todos: Todo[];
    changeDoneFlagCallBack: (id: number, done: boolean) => void;
    handleDeleteCallBack: (id: number) => void;
}

const TodoList: React.FC<TodoListProps> = ({todos, changeDoneFlagCallBack, handleDeleteCallBack}) => {
    return (
        <TodoListContainer>
            {todos.map((todo) => (
                <TodoItem
                    key={todo.id}
                    {...todo}
                    changeDoneFlagCallBack={changeDoneFlagCallBack}
                    handleDeleteCallBack={handleDeleteCallBack}
                />
            ))}
        </TodoListContainer>
    );
};

export const TodoListContainer = styled.div`
    width: 600px;
    margin: 0 auto;
`;

export default TodoList;
