from fastapi import FastAPI
from pydantic import BaseModel
from transformers import AutoTokenizer

app = FastAPI()

tokenizers = {
    "bert": AutoTokenizer.from_pretrained("bert-base-uncased"),
    "roberta": AutoTokenizer.from_pretrained("roberta-base"),
    "gpt2": AutoTokenizer.from_pretrained("gpt2"),
    "llama": AutoTokenizer.from_pretrained("huggyllama/llama-30b"),
    "t5": AutoTokenizer.from_pretrained("google/t5-v1_1-xl"),
}

class TokenRequest(BaseModel):
    Text: str
    Model: str

@app.post("/tokenize")
def tokenize(req: TokenRequest):
    model = req.Model.lower()
    if model not in tokenizers:
        return {"Error": "Nieznay model. Wybierz jeden z listy dostępnych modeli."}

    tokenizer = tokenizers[model]
    tokens = tokenizer.tokenize(req.Text)
    # Ensure tokens are strings and use the correct property name
    tokens_list = [str(token) for token in tokens]
    return {"Tokens": tokens_list}
