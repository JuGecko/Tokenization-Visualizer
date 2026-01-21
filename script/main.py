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

    # if model == "gpt-2": model = "gpt-2"

    if model not in tokenizers:
        found = False
        for key in tokenizers.keys():
            if key.lower() == model:
                model = key
                found = True
                break
        if not found:
            return {"Error": f"Model '{model}' not found."}

    tokenizer = tokenizers[model]

    tokens = tokenizer.tokenize(req.Text)
    ids = tokenizer.convert_tokens_to_ids(tokens)
    tokens_list = [str(token) for token in tokens]

    return {
        "Tokens": tokens_list,
        "Ids": ids
    }