function MessageList({messages})
{
    return(
        <div className="message-List">
            {messages.map((message,index)=>(
                <div key={index} 
                    className={`message-row ${message.role}`}>
                    <div className="message-bubble">
                        {message.content}
                    </div>
                    {/* {message.role === 'user' ? 'User: ':'AI: '} */}
                </div>
            ))}
        </div>
    )
}
export default MessageList