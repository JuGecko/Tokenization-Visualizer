from fastapi import FastAPI
from pydantic import BaseModel
from transformers import AutoTokenizer

app = FastAPI()

tokenizers = {
    "bert": AutoTokenizer.from_pretrained("bert-base-uncased"),
    "gpt-2": AutoTokenizer.from_pretrained("gpt2"),
    "xml-roberta": AutoTokenizer.from_pretrained("xlm-roberta-base"),
    "llama": AutoTokenizer.from_pretrained("huggyllama/llama-7b"),
}

class TokenRequest(BaseModel):
    Text: str
    Model: str

@app.post("/tokenize")
def tokenize(req: TokenRequest):
    model = req.Model.lower()
    if model not in tokenizers:
        return {"Error": "Nieznany model. Wybierz jeden z listy dostępnych modeli."}

    tokenizer = tokenizers[model]
    tokens = tokenizer.tokenize(req.Text)
    # Ensure tokens are strings and use the correct property name
    tokens_list = [str(token) for token in tokens]
    return {"Tokens": tokens_list}
