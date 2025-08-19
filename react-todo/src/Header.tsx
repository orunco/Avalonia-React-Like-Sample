import React from "react";
import styled from 'styled-components';

interface HeaderProps {
    onEnter: (value: string) => void;
}

const Header: React.FC<HeaderProps> = ({onEnter}) => {
    const handleKeyUp = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            onEnter(e.currentTarget.value);
            e.currentTarget.value = '';
        }
    };

    return (
        <HeaderContainer>
            <input
                type="text"
                placeholder="请输入你的任务名称，按回车键确认"
                onKeyUp={handleKeyUp}
            />
        </HeaderContainer>
    );
};

export const HeaderContainer = styled.header`
    input {
        width: 560px;
        height: 28px;
        font-size: 14px;
        border: 1px solid #ccc;
        border-radius: 4px;
        padding: 4px 7px;

        &:focus {
            outline: none;
            border-color: rgba(82, 168, 236, 0.8);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075),
            0 0 8px rgba(82, 168, 236, 0.6);
        }
    }
`;

export default Header;
