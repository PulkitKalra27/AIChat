function ChatInput({message, setMessage, handleSend}) {
    return(
        <div className="chat-input">
            <input
                value={message} 
                placeholder="Type your message"
                onChange={(event) => setMessage(event.target.value)}></input>
            
            <button onClick={handleSend}>Send</button>
            
        </div>
    )
}
export default ChatInput