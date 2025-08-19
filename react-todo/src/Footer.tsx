import React from "react";
import styled from 'styled-components';

interface Todo {
    id: number;
    name: string;
    done: boolean;
}

interface FooterProps {
    todos: Todo[];
    allDoneCallBack: (flag: boolean) => void;
    removeAllDoneCallBack: () => void;
}

const Footer: React.FC<FooterProps> = ({todos, allDoneCallBack, removeAllDoneCallBack}) => {
    const handleChangeAll = (e: React.ChangeEvent<HTMLInputElement>) => {
        allDoneCallBack(e.target.checked);
    };

    const OKCount = todos.filter(todo => todo.done).length;
    const total = todos.length;

    return (
        <FooterContainer>
            <label>
                <input
                    type="checkbox"
                    checked={OKCount === total && total !== 0}
                    onChange={handleChangeAll}
                />
            </label>
            <span>
                <span>已完成{OKCount}</span>/全部{total}
            </span>
            <StyledButton onClick={removeAllDoneCallBack}>
                清除已完成任务
            </StyledButton>
        </FooterContainer>
    );
};

export const FooterContainer = styled.footer`
    height: 40px;
    line-height: 40px;
    padding-left: 6px;
    margin-top: 5px;

    label {
        display: inline-block;
        margin-right: 20px;
        cursor: pointer;

        input {
            position: relative;
            top: -1px;
            vertical-align: middle;
            margin-right: 5px;
        }
    }

    button {
        float: right;
        margin-top: 5px;
    }
`;

const StyledButton = styled.button`
    display: inline-block;
    padding: 4px 12px;
    margin-bottom: 0;
    font-size: 14px;
    line-height: 20px;
    text-align: center;
    vertical-align: middle;
    cursor: pointer;
    box-shadow: inset 0 1px 0 rgba(255, 255, 255, 0.2), 0 1px 2px rgba(0, 0, 0, 0.05);
    border-radius: 4px;

    &:focus {
        outline: none;
    }
`;

export default Footer;
