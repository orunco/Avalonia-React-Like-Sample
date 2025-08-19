import React from "react";
import styled from 'styled-components';

interface TodoItemProps {
    id: number;
    name: string;
    done: boolean;
    changeDoneFlagCallBack: (id: number, done: boolean) => void;
    handleDeleteCallBack: (id: number) => void;
}

const TodoItem: React.FC<TodoItemProps> = ({id, name, done, changeDoneFlagCallBack, handleDeleteCallBack}) => {
    const handleCheck = (e: React.ChangeEvent<HTMLInputElement>) => {
        changeDoneFlagCallBack(id, e.target.checked);
    };

    const handleDelete = () => {
        if (confirm("确定删除吗？")) {
            handleDeleteCallBack(id);
        }
    };

    return (
        <TodoItemWrapper>
            <label>
                <input
                    type="checkbox"
                    id={id.toString()}
                    checked={done}
                    onChange={handleCheck}
                />
                <span>{name}</span>
            </label>
            <DeleteButton onClick={handleDelete}>
                删除
            </DeleteButton>
        </TodoItemWrapper>
    );
};

export const TodoItemWrapper = styled.li`
    list-style: none;
    height: 36px;
    line-height: 36px;
    padding: 0 5px;
    border-bottom: 1px solid #ddd;
    background: white;
    transition: all 0.3s;

    &:hover {
        background: #f0f0f0;
    }

    &:last-child {
        border-bottom: none;
    }

    label {
        float: left;
        cursor: pointer;

        input {
            vertical-align: middle;
            margin-right: 6px;
            position: relative;
            top: -1px;
        }
    }
`;

export const DeleteButton = styled.button`
    color: #fff;
    background-color: #dc3545;
    border-color: #dc3545;
    padding: 0.25rem 0.5rem;
    font-size: 0.875rem;
    line-height: 1.5;
    border-radius: 0.2rem;
    float: right;
    opacity: 0;
    transition: opacity 0.3s;
    height: 24px;
    margin-top: 6px;
    display: inline-block !important;

    ${TodoItemWrapper}:hover & {
        opacity: 1 !important;
    }
`;

export default TodoItem;
